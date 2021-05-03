using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers.UserProfilePictureUrlUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfilePictureUrlUpdatedDomainEventHandler"/> class.
    /// </summary>
    public class UserProfilePictureUrlUpdatedDomainEventHandler : INotificationHandler<UserProfilePictureUrlUpdatedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePictureUrlUpdatedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public UserProfilePictureUrlUpdatedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the user profile picture url updated domain event and enqueues the user profile picture url updated integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(UserProfilePictureUrlUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            UserProfilePictureUrlUpdatedIntegrationEvent userProfilePictureUrlUpdatedIntegrationEvent = new(notification.UserId, notification.ProfilePictureUrl);
            userProfilePictureUrlUpdatedIntegrationEvent.SetExchangeName("UserExchange");
            _integrationEventService.EnqueueEvent(userProfilePictureUrlUpdatedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
