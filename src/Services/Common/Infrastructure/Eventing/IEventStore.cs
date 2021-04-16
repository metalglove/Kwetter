using Kwetter.Services.Common.EventBus.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Eventing
{
    /// <summary>
    /// Represents the <see cref="IEventStore"/> interface.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Saves the events to the event store asynchronously.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task SaveEventsAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken);

        /// <summary>
        /// Saves the event to the event store asynchronously.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task SaveEventAsync(IEvent @event, CancellationToken cancellationToken);

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
