using Kwetter.Services.Common.Domain.Events;
using Kwetter.Services.Common.EventBus.Abstractions;
using Kwetter.Services.Common.Infrastructure.Eventing;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers
{
    /// <summary>
    /// Represents the <see cref="AnyDomainEventHandler{T}"/> class.
    /// </summary>
    /// <typeparam name="TDomainEvent"></typeparam>
    public sealed class AnyDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : DomainEvent
    {
        private readonly IEventBus _eventBus;
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyDomainEventHandler{TDomainEvent}"/> class.
        /// </summary>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="eventStore">The event store.</param>
        public AnyDomainEventHandler(IEventBus eventBus, IEventStore eventStore)
        {
            _eventBus = eventBus;
            _eventStore = eventStore;
        }

        /// <summary>
        /// Handles the domain event by publishing the domain event to the "DomainEvents" message queue and storing the message in the event store.
        /// </summary>
        /// <param name="notification">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
        {
            _eventBus.Publish(notification, "DomainEvents");
            await _eventStore.SaveEventAsync(notification, cancellationToken);
        }
    }
}
