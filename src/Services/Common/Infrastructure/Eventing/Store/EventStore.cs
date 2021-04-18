using EventStore.Client;
using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Store;
using Kwetter.Services.Common.Domain.Events;
using Kwetter.Services.Common.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Eventing.Store
{
    /// <summary>
    /// Represents the <see cref="EventStore"/> class.
    /// </summary>
    public sealed class EventStore : IEventStore
    {
        private readonly ServiceConfiguration _serviceConfiguration;
        private readonly ConcurrentQueue<EventData> _events;
        private readonly EventStoreClient _eventStoreClient;
        private readonly ILogger<EventStore> _logger;
        private readonly IEventSerializer _eventSerializer;
        private readonly string _eventStream;
        private object transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStore"/> class.
        /// </summary>
        /// <param name="serviceConfigurationOptions">The service configuration options.</param>
        /// <param name="eventStoreClient">The event store client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="eventSerializer">The event serializer.</param>
        public EventStore(
            IOptions<ServiceConfiguration> serviceConfigurationOptions,
            EventStoreClient eventStoreClient,
            ILogger<EventStore> logger,
            IEventSerializer eventSerializer)
        {
            _serviceConfiguration = serviceConfigurationOptions.Value ?? throw new AggregateException(nameof(serviceConfigurationOptions));
            _eventStoreClient = eventStoreClient ?? throw new ArgumentNullException(nameof(eventStoreClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
            _eventStream = $"DomainEvents-{_serviceConfiguration.ShortTitle}";
            _events = new ConcurrentQueue<EventData>();
        }

        /// <inheritdoc cref="IEventStore.CommitTransactionAsync(CancellationToken)"/>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            if (transaction is null)
                throw new Exception("Attempted to commit the transaction while there is not an active transaction.");

            if (_events.IsEmpty)
            {
                _logger.LogInformation($"No events to commit.");
                return;
            }

            await _eventStoreClient.AppendToStreamAsync(_eventStream, StreamState.Any, GetEvents(), cancellationToken: cancellationToken);
            
            transaction = null;
            _logger.LogInformation($"Completed a transaction for the EventStore on the {_eventStream} stream.");

            IEnumerable<EventData> GetEvents()
            {
                while (!_events.IsEmpty)
                {
                    _events.TryDequeue(out EventData @event);
                    _logger.LogInformation($"Committed event: {@event.Type}:{@event.EventId}");
                    yield return @event;
                }
            }
        }

        /// <inheritdoc cref="IEventStore.RollbackTransaction()"/>
        public void RollbackTransaction()
        {
            if (transaction is null)
                throw new Exception("Attempted to rollback the transaction while there is not an active transaction.");

            _events.Clear();
            transaction = null;
        }

        /// <inheritdoc cref="IEventStore.SaveEvent{TEvent}(TEvent)"/>
        public void SaveEvent<TEvent>(TEvent @event) where TEvent : DomainEvent
        {
            if (transaction is null)
                throw new Exception("Attempted to save an event while there is not an active transaction.");

            // NOTE: recommended by EventStore to use camelcase event names.
            string camelCaseEventName = @event.Name.ToCamelCase();
            _events.Enqueue(new EventData(
                    eventId: Uuid.FromGuid(@event.Id),
                    type: camelCaseEventName,
                    contentType: "application/json",
                    data: _eventSerializer.Serialize(@event),
                    metadata: _eventSerializer.Serialize(new { @event.Version, @event.Name })));
            _logger.LogInformation($"Enqueued event: {camelCaseEventName}:{@event.Id}");
        }

        /// <inheritdoc cref="IEventStore.SaveEvents(IEnumerable{IEvent})"/>
        public void SaveEvents<TEvent>(IEnumerable<TEvent> events) where TEvent : DomainEvent
        {
            foreach (TEvent @event in events)
                SaveEvent(@event);
        }

        /// <inheritdoc cref="IEventStore.StartTransactionAsync(CancellationToken)"/>
        public Task StartTransactionAsync(CancellationToken cancellationToken)
        {
            if (transaction is not null)
                throw new Exception("Attempted to start a transaction while another transaction is still active.");

            transaction = new object();
            _logger.LogInformation($"Started a transaction for the EventStore on the {_eventStream} stream.");
            cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="IDisposable.Dispose()"/>
        public void Dispose()
        {
            transaction = null;
        }
    }
}
