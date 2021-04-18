using System;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.Common.Domain.Events;

namespace Kwetter.Services.Common.Application.Eventing
{
    /// <summary>
    /// Represents the <see cref="IEventHandler{TEvent}"/> interface.
    /// </summary>
    /// <typeparam name="TEvent">The type of event.</typeparam>
    public interface IEventHandler<in TEvent> where TEvent : class, IEvent
    {
        /// <summary>
        /// The unsubscribe event.
        /// </summary>
        public event EventHandler UnsubscribeEventHandler;

        /// <summary>
        /// Unsubscribes from the event bus.
        /// </summary>
        public void Unsubscribe();

        /// <summary>
        /// Handles the event asynchronously.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="ValueTask"/>.</returns>
        public ValueTask HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}