using Kwetter.Services.Common.Domain.Events;

namespace Kwetter.Services.Common.Application.Eventing
{
    /// <summary>
    /// Represents the <see cref="IEventPublisher"/> interface.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes the event to the event bus.
        /// </summary>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <param name="event">The event to publish.</param>
        /// <param name="exchangeName">The name of the exchange to publish to.</param>
        /// <param name="routingKey">The routing key to use.</param>
        public void Publish<TEvent>(TEvent @event, string exchangeName, string routingKey) where TEvent : Event;
    }
}