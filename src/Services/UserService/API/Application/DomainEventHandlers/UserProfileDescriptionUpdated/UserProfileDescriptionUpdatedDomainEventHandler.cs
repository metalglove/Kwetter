using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers.UserProfileDescriptionUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfileDescriptionUpdatedDomainEventHandler"/> class.
    /// </summary>
    public class UserProfileDescriptionUpdatedDomainEventHandler : INotificationHandler<UserProfileDescriptionUpdatedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileDescriptionUpdatedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public UserProfileDescriptionUpdatedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the user profile description updated domain event and enqueues the user profile description updated integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(UserProfileDescriptionUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            UserProfileDescriptionUpdatedIntegrationEvent userProfileDescriptionUpdatedIntegrationEvent = new(notification.UserId, notification.UserProfileDescription);
            userProfileDescriptionUpdatedIntegrationEvent.SetExchangeName("UserExchange");
            _integrationEventService.EnqueueEvent(userProfileDescriptionUpdatedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
