using Kwetter.Services.Common.Application.Eventing.Store;
using Kwetter.Services.Common.Domain.Events;
using Kwetter.Services.Common.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Tests.Mocks
{
    public class EventStoreMock : IEventStore
    {
        private readonly List<IEvent> _events = new();
        private readonly List<IEvent> _committedEvents = new();
        private readonly ILogger<EventStoreMock> _logger;
        private object transaction;

        public IReadOnlyList<IEvent> Events => _events.AsReadOnly();
        public IReadOnlyList<IEvent> CommittedEvents => _committedEvents.AsReadOnly();

        public EventStoreMock(ILogger<EventStoreMock> logger)
        {
            _logger = logger;
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            if (transaction is null)
                throw new Exception("Attempted to commit a transaction while the transaction is null.");

            if (!_events.Any())
            {
                _logger.LogInformation($"No events to commit.");
                _logger.LogInformation($"Completed event store transaction.");
                return Task.CompletedTask;
            }

            foreach (IEvent @event in _events)
            {
                _committedEvents.Add(@event);
                _logger.LogInformation($"Committed event: {@event.Name.ToCamelCase()}:{@event.Id}");
            }
            _events.Clear();
            _logger.LogInformation($"Completed event store transaction.");
            transaction = null;
            return Task.CompletedTask;
        }

        public void RollbackTransaction()
        {
            _events.Clear();
            transaction = null;
        }

        public Task StartTransactionAsync(CancellationToken cancellationToken)
        {
            if (transaction is not null)
                throw new Exception("Attempted to start a transaction while another transaction is still active.");

            transaction = new object();
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation($"Started event store transaction.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            transaction = null;
        }

        public void SaveEvents<TEvent>(IEnumerable<TEvent> events) where TEvent : DomainEvent
        {
            foreach (TEvent @event in events)
                SaveEvent(@event);
        }

        public void SaveEvent<TEvent>(TEvent @event) where TEvent : DomainEvent
        {
            if (transaction is null)
                throw new Exception("Attempted to save an event while there is not an active transaction.");
            _events.Add(@event);
            _logger.LogInformation($"Enqueued event: {@event.Name.ToCamelCase()}:{@event.Id}");
        }
    }
}
