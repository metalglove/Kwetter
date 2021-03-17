using Kwetter.Services.Common.EventBus.Abstractions;
using Kwetter.Services.Common.EventBus.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventLogService"/> class.
    /// </summary>
    public class IntegrationEventLogService : IIntegrationEventLogService, IDisposable
    {
        private readonly IMessageSerializer _messageSerializer;
        private readonly IntegrationEventLogDbContext _integrationEventLogDbContext;
        private readonly List<Type> _eventTypes;
        private volatile bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventLogService"/> class.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="integrationEventLogDbContext">The integration event log database context.</param>
        /// <param name="assembly">The assembly containing the integration event types.</param>
        public IntegrationEventLogService(
            IMessageSerializer messageSerializer,
            IntegrationEventLogDbContext integrationEventLogDbContext, 
            Assembly assembly)
        {
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            _integrationEventLogDbContext = integrationEventLogDbContext ?? throw new ArgumentNullException(nameof(integrationEventLogDbContext));
            _eventTypes = assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        /// <inheritdoc cref="IIntegrationEventLogService.RetrieveEventLogsPendingToPublishAsync(Guid, CancellationToken)"/>
        public async ValueTask<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            List<IntegrationEventLogEntry> result = await _integrationEventLogDbContext.IntegrationEventLogEntries
                .Where(e => e.TransactionId == transactionId && e.State == IntegrationEventState.NotPublished)
                .ToListAsync(cancellationToken: cancellationToken);
            if (result != null && result.Any())
            {
                return result.OrderBy(o => o.CreationDateTime)
                    .Select(e =>
                    {
                        // Deserializes the integration event from the integration event log.
                        e.IntegrationEvent = _messageSerializer.Deserialize(Encoding.UTF8.GetBytes(e.Content), _eventTypes.Find(t => t.Name == e.EventTypeShortName)) as IntegrationEvent;
                        return e;
                    });
            }
            return new List<IntegrationEventLogEntry>();
        }

        /// <inheritdoc cref="IIntegrationEventLogService.SaveEventAsync(IntegrationEvent, IDbContextTransaction, CancellationToken)"/>
        public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            if (transaction == null) 
                throw new ArgumentNullException(nameof(transaction));
            IntegrationEventLogEntry eventLogEntry = new(@event, Encoding.UTF8.GetString(_messageSerializer.Serialize(@event).Span), transaction.TransactionId);
            _integrationEventLogDbContext.Database.UseTransaction(transaction.GetDbTransaction());
            _integrationEventLogDbContext.IntegrationEventLogEntries.Add(eventLogEntry);
            return _integrationEventLogDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc cref="IIntegrationEventLogService.MarkEventAsPublishedAsync(Guid, CancellationToken)"/>
        public Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return UpdateEventStatus(eventId, IntegrationEventState.Published, cancellationToken);
        }

        /// <inheritdoc cref="IIntegrationEventLogService.MarkEventAsInProgressAsync(Guid, CancellationToken)"/>
        public Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return UpdateEventStatus(eventId, IntegrationEventState.InProgress, cancellationToken);
        }

        /// <inheritdoc cref="IIntegrationEventLogService.MarkEventAsFailedAsync(Guid, CancellationToken)"/>
        public Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return UpdateEventStatus(eventId, IntegrationEventState.PublishedFailed, cancellationToken);
        }

        private Task UpdateEventStatus(Guid eventId, IntegrationEventState status, CancellationToken cancellationToken)
        {
            IntegrationEventLogEntry eventLogEntry = _integrationEventLogDbContext.IntegrationEventLogEntries.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;
            if (status == IntegrationEventState.InProgress)
                eventLogEntry.TimesSent++;
            _integrationEventLogDbContext.IntegrationEventLogEntries.Update(eventLogEntry);
            return _integrationEventLogDbContext.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) 
                return;
            if (disposing) 
                _integrationEventLogDbContext?.Dispose();
            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
