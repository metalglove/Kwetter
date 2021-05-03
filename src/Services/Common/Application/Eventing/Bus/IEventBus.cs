using Kwetter.Services.Common.Domain.Events;

namespace Kwetter.Services.Common.Application.Eventing.Bus
{
    /// <summary>
    /// Represents the <see cref="IEventBus"/> interface.
    /// </summary>
    public interface IEventBus : IEventPublisher
    {
        /// <summary>
        /// Declares the exchange on the event bus.
        /// </summary>
        /// <param name="exchangeName">The exchange name.</param>
        /// <param name="exchangeType">The exchange type.</param>
        public void DeclareExchange(string exchangeName, ExchangeType exchangeType);

        /// <summary>
        /// Subscribes to the event bus using a specified queue name.
        /// </summary>
        /// <param name="exchangeName">The name of the exchange to subscribe to.</param>
        /// <param name="queueName">The name of the queue to subscribe to.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <typeparam name="TEventHandler">The type of event handler.</typeparam>
        public void Subscribe<TEvent, TEventHandler>(string exchangeName, string queueName, TEventHandler eventHandler) where TEvent : Event where TEventHandler : IEventHandler<TEvent>;

        /// <summary>
        /// Unsubscribe from the event bus.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <typeparam name="TEventHandler">The type of event handler.</typeparam>
        public void Unsubscribe<TEvent, TEventHandler>(TEventHandler eventHandler) where TEvent : Event where TEventHandler : IEventHandler<TEvent>;
    }
}
