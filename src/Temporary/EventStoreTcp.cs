using EventStore.ClientAPI;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Store;
using Kwetter.Services.Common.Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Eventing.Store
{
    /// <summary>
    /// Represents the <see cref="EventStoreTcp"/> class.
    /// </summary>
    public sealed class EventStoreTcp : IEventStore, IDisposable
    {
        private readonly IAsyncFactory<IEventStoreConnection> _eventStoreConnectionFactory;
        private readonly ILogger<EventStoreTcp> _logger;
        private readonly IEventSerializer _eventSerializer;
        private EventStoreTransaction eventStoreTransaction;
        private readonly string _eventStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreTcp"/> class.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <param name="eventStoreConnectionFactory">The event store connection factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="eventSerializer">The event serializer.</param>
        public EventStoreTcp(
            string serviceName,
            IAsyncFactory<IEventStoreConnection> eventStoreConnectionFactory,
            ILogger<EventStoreTcp> logger,
            IEventSerializer eventSerializer)
        {
            _eventStream = $"DomainEvents-{serviceName}";
            _eventStoreConnectionFactory = eventStoreConnectionFactory;
            _logger = logger;
            _eventSerializer = eventSerializer;
        }

        /// <inheritdoc cref="IEventStore.StartTransactionAsync(CancellationToken)"/>
        public async Task StartTransactionAsync(CancellationToken cancellationToken)
        {
            IEventStoreConnection eventStoreConnection = await _eventStoreConnectionFactory.CreateAsync(cancellationToken);
            if (eventStoreTransaction is not null)
                throw new Exception("Attempted to start a transaction while another transaction is still active.");
            eventStoreTransaction = await eventStoreConnection.StartTransactionAsync(_eventStream, ExpectedVersion.Any);
            _logger.LogInformation($"Started a transaction for the EventStore on the {_eventStream} stream.");
            cancellationToken.ThrowIfCancellationRequested();
        }

        /// <inheritdoc cref="IEventStore.CommitTransactionAsync(CancellationToken)"/>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            if (eventStoreTransaction is null)
                throw new Exception("Attempted to commit a transaction while the transaction is null.");
            await eventStoreTransaction.CommitAsync();
            _logger.LogInformation($"Committed a transaction for the EventStore on the {_eventStream} stream.");
            eventStoreTransaction.Dispose();
            eventStoreTransaction = null;
            cancellationToken.ThrowIfCancellationRequested();
        }

        /// <inheritdoc cref="IEventStore.RollbackTransaction()"/>
        public void RollbackTransaction()
        {
            if (eventStoreTransaction is null)
                throw new Exception("Attempted to rollback a transaction while the transaction is null.");
            eventStoreTransaction.Dispose();
            eventStoreTransaction = null;
            _logger.LogInformation($"Rolledback a transaction for the EventStore on the {_eventStream} stream.");
        }

        /// <inheritdoc cref="IEventStore.SaveEventsAsync(IEnumerable{IEvent}, CancellationToken)"/>
        public async Task SaveEventsAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken)
        {
            IEnumerable<EventData> eventData = events.Select(@event => 
            {
                return new EventData(
                    eventId: @event.Id,
                    type: @event.Name,
                    isJson: true,
                    data: _eventSerializer.Serialize(@event).ToArray(),
                    metadata: _eventSerializer.Serialize(new { @event.Version, @event.Name }).ToArray());
            });
            await eventStoreTransaction.WriteAsync(eventData);
            cancellationToken.ThrowIfCancellationRequested();
        }
     
        /// <inheritdoc cref="IEventStore.SaveEventAsync(IEvent, CancellationToken)"/>
        public async Task SaveEventAsync(IEvent @event, CancellationToken cancellationToken)
        {
            EventData eventData = new(
                    eventId: @event.Id,
                    type: @event.Name,
                    isJson: true,
                    data: _eventSerializer.Serialize(@event).ToArray(),
                    metadata: _eventSerializer.Serialize(new { @event.Version, @event.Name }).ToArray());
            await eventStoreTransaction.WriteAsync(eventData);
            cancellationToken.ThrowIfCancellationRequested();
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (eventStoreTransaction is null)
                return;
            ((IDisposable)eventStoreTransaction).Dispose();
        }
    }
}
