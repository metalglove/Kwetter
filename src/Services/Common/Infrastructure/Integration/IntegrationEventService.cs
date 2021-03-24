using Kwetter.Services.Common.EventBus.Abstractions;
using Kwetter.Services.Common.EventBus.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventService{TContext}"/> class.
    /// </summary>
    /// <typeparam name="TContext">The database context of the service.</typeparam>
    public sealed class IntegrationEventService<TContext> : IIntegrationEventService where TContext : UnitOfWork<TContext>
    {
        private readonly IntegrationEventMessagingConfiguration _integrationEventMessagingConfiguration;
        private readonly ILogger<IntegrationEventService<TContext>> _logger;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly TContext _unitOfWork;
        private readonly IEventBus _eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventService{TContext}"/> class.
        /// </summary>
        /// <param name="integrationEventMessagingConfiguration">The integration event messaging configuration.</param>
        /// <param name="integrationEventLogService">The integration event log service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWork">The unit of work for the service.</param>
        /// <param name="eventBus">The event bus.</param>
        public IntegrationEventService(
            IOptions<IntegrationEventMessagingConfiguration> integrationEventMessagingConfiguration,
            IIntegrationEventLogService integrationEventLogService,
            ILogger<IntegrationEventService<TContext>> logger,
            TContext unitOfWork,
            IEventBus eventBus)
        {
            _integrationEventMessagingConfiguration = integrationEventMessagingConfiguration.Value ?? throw new ArgumentNullException(nameof(integrationEventMessagingConfiguration)); ;
            _eventLogService = integrationEventLogService ?? throw new ArgumentNullException(nameof(integrationEventLogService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc cref="IIntegrationEventService.PublishEventsThroughEventBusAsync(Guid, CancellationToken)"/>
        public async Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            IEnumerable<IntegrationEventLogEntry> pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId, cancellationToken);
            foreach (IntegrationEventLogEntry eventLogEntry in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", eventLogEntry.EventId, _integrationEventMessagingConfiguration.ServiceName, eventLogEntry.IntegrationEvent);
                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(eventLogEntry.EventId, cancellationToken);
                    _eventBus.Publish(eventLogEntry.IntegrationEvent, _integrationEventMessagingConfiguration.MessageQueueName);
                    await _eventLogService.MarkEventAsPublishedAsync(eventLogEntry.EventId, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", eventLogEntry.EventId, _integrationEventMessagingConfiguration.ServiceName);
                    await _eventLogService.MarkEventAsFailedAsync(eventLogEntry.EventId, cancellationToken);
                }
            }
        }

        /// <inheritdoc cref="IIntegrationEventService.AddAndSaveEventAsync(IntegrationEvent, CancellationToken)"/>
        public async Task AddAndSaveEventAsync(IntegrationEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Enqueueing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", @event.Id, @event);
            await _eventLogService.SaveEventAsync(@event, _unitOfWork.CurrentTransaction, cancellationToken);
        }
    }
}
