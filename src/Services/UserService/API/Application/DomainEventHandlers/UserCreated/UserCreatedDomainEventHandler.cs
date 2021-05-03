using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers.UserCreated
{
    /// <summary>
    /// Represents the <see cref="UserCreatedDomainEventHandler"/> class.
    /// </summary>
    public sealed class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public UserCreatedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the user created domain event and enqueues the user created integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            UserCreatedIntegrationEvent userCreatedIntegrationEvent = new(
                notification.UserId,
                notification.UserDisplayName,
                notification.UserProfileDescription,
                notification.UserProfilePictureUrl);
            userCreatedIntegrationEvent.SetExchangeName("UserExchange");
            _integrationEventService.EnqueueEvent(userCreatedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
