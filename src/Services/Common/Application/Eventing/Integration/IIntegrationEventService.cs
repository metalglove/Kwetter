namespace Kwetter.Services.Common.Application.Eventing.Integration
{
    /// <summary>
    /// Represents the <see cref="IIntegrationEventService"/> interface.
    /// Used for publishing integration events.
    /// </summary>
    public interface IIntegrationEventService
    {
        /// <summary>
        /// Publishes integration events through the event bus.
        /// </summary>
        public void PublishEvents();

        /// <summary>
        /// Enqueues the integration event to the queue.
        /// </summary>
        /// <param name="event">The integration event.</param>
        public void EnqueueEvent(IntegrationEvent @event);
    }
}
