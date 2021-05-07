using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.DomainEventHandlers.UserFollowed
{
    /// <summary>
    /// Represents the <see cref="UserFollowedDomainEventHandler"/> class.
    /// </summary>
    public sealed class UserFollowedDomainEventHandler : INotificationHandler<FollowDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFollowedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public UserFollowedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the follow domain event and enqueues the user followed integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(FollowDomainEvent notification, CancellationToken cancellationToken)
        {
            UserFollowedIntegrationEvent userFollowedIntegrationEvent = new(notification.FollowingId, notification.FollowerId, notification.FollowedDateTime);
            userFollowedIntegrationEvent.SetExchangeName("FollowExchange");
            userFollowedIntegrationEvent.SetExchangeName("FollowService.UserFollowedIntegrationEvent");
            _integrationEventService.EnqueueEvent(userFollowedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
