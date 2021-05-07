using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers.UserDisplayNameUpdated
{
    /// <summary>
    /// Represents the <see cref="UserDisplayNameUpdatedDomainEventHandler"/> class.
    /// </summary>
    public class UserDisplayNameUpdatedDomainEventHandler : INotificationHandler<UserDisplayNameUpdatedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisplayNameUpdatedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public UserDisplayNameUpdatedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the user display name updated domain event and enqueues the user display name updated integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(UserDisplayNameUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            UserDisplayNameUpdatedIntegrationEvent userDisplayNameUpdatedIntegrationEvent = new(notification.UserId, notification.UserDisplayName);
            userDisplayNameUpdatedIntegrationEvent.SetExchangeName("UserExchange");
            userDisplayNameUpdatedIntegrationEvent.SetRoutingKey("UserService.UserDisplayNameUpdatedIntegrationEvent");
            _integrationEventService.EnqueueEvent(userDisplayNameUpdatedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
