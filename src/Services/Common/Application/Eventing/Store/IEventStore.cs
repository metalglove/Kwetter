using Kwetter.Services.Common.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Application.Eventing.Store
{
    /// <summary>
    /// Represents the <see cref="IEventStore"/> interface.
    /// </summary>
    public interface IEventStore : IDisposable
    {
        /// <summary>
        /// Saves the events to the event store.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <typeparam name="TEvent">The event type.</typeparam>
        public void SaveEvents<TEvent>(IEnumerable<TEvent> events) where TEvent : DomainEvent;

        /// <summary>
        /// Saves the event to the event store.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <typeparam name="TEvent">The event type.</typeparam>
        public void SaveEvent<TEvent>(TEvent @event) where TEvent : DomainEvent;

        /// <summary>
        /// Starts the transaction for the event store asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task StartTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token/</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task CommitTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction();
    }
}
