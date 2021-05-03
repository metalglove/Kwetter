using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.DomainEventHandlers.UserUnfollowed
{
    /// <summary>
    /// Represents the <see cref="UserUnfollowedDomainEventHandler"/> class.
    /// </summary>
    public sealed class UserUnfollowedDomainEventHandler : INotificationHandler<UnfollowDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUnfollowedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public UserUnfollowedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the unfollow domain event and enqueues the user unfollowed integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(UnfollowDomainEvent notification, CancellationToken cancellationToken)
        {
            UserUnfollowedIntegrationEvent userUnfollowedIntegrationEvent = new(notification.FollowingId, notification.FollowerId, notification.UnfollowedDateTime);
            userUnfollowedIntegrationEvent.SetExchangeName("FollowExchange");
            _integrationEventService.EnqueueEvent(userUnfollowedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
