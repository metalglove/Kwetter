using Kwetter.Services.Common.Application.Eventing.Store;
using Kwetter.Services.Common.Domain.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Eventing
{
    /// <summary>
    /// Represents the <see cref="AnyDomainEventHandler{T}"/> class.
    /// </summary>
    /// <typeparam name="TDomainEvent">The domain event type/</typeparam>
    public sealed class AnyDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : DomainEvent
    {
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyDomainEventHandler{TDomainEvent}"/> class.
        /// </summary>
        /// <param name="eventStore">The event store.</param>
        public AnyDomainEventHandler(IEventStore eventStore)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        /// <summary>
        /// Handles the domain event by publishing the domain event to the "DomainEvents" stream in the event store.
        /// </summary>
        /// <param name="notification">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
        {
            _eventStore.SaveEvent(notification);
            cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }
    }
}
