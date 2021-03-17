namespace Kwetter.Services.Common.EventBus.Abstractions
{
    /// <summary>
    /// Represents the <see cref="IEventBus"/> interface.
    /// </summary>
    public interface IEventBus : IEventPublisher
    {
        /// <summary>
        /// Subscribes to the event bus.
        /// </summary>
        /// <param name="queueName">The name of the queue to subscribe to.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <typeparam name="TEventHandler">The type of event handler.</typeparam>
        void Subscribe<TEvent, TEventHandler>(string queueName, TEventHandler eventHandler) where TEvent : class, IEvent where TEventHandler : IEventHandler<TEvent>;

        /// <summary>
        /// Unsubscribe from the event bus.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <typeparam name="TEventHandler">The type of event handler.</typeparam>
        void Unsubscribe<TEvent, TEventHandler>(TEventHandler eventHandler) where TEvent : class, IEvent where TEventHandler : IEventHandler<TEvent>;
    }
}
