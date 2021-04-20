﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.Common.Domain.Events;

namespace Kwetter.Services.Common.Application.Eventing
{
    /// <summary>
    /// Represents the base <see cref="KwetterEventHandler{TEvent}"/> class.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    public abstract class KwetterEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : Event
    {
        private EventHandler _unsubscribeEventHandler;

        /// <summary>
        /// Gets the unsubscribe event handler.
        /// </summary>
        public event EventHandler UnsubscribeEventHandler
        {
            add => _unsubscribeEventHandler += value;
            remove => _unsubscribeEventHandler -= value;
        }

        /// <inheritdoc cref="IEventHandler{TEvent}.HandleAsync(TEvent, CancellationToken)"/>
        public abstract ValueTask HandleAsync(TEvent @event, CancellationToken cancellationToken);

        /// <inheritdoc cref="IEventHandler{TEvent}.Unsubscribe()"/>
        public void Unsubscribe()
        {
            _unsubscribeEventHandler?.Invoke(this, new EventArgs());
        }
    }
}
