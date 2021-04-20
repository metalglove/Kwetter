﻿using Kwetter.Services.Common.Domain.Events;

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
        /// <param name="queueName">The name of the queue to publish to.</param>
        public void Publish<TEvent>(TEvent @event, string queueName) where TEvent : Event;
    }
}