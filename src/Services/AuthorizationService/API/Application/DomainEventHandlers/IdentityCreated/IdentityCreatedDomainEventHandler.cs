using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate.Events;
using Kwetter.Services.Common.Application.Eventing.Integration;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.API.Application.DomainEventHandlers.IdentityCreated
{
    /// <summary>
    /// Represents the <see cref="IdentityCreatedDomainEventHandler"/> class.
    /// Transforms the domain event to an integration event for the profile service to know about the newly created identity.
    /// </summary>
    public sealed class IdentityCreatedDomainEventHandler : INotificationHandler<IdentityCreatedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityCreatedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public IdentityCreatedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the identity created domain event and enqueues the identity created integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(IdentityCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new(
                userId: notification.UserId,
                givenName: notification.GivenName,
                userName: notification.UserName,
                profilePictureUrl: notification.ProfilePictureUrl
            );
            identityCreatedIntegrationEvent.SetExchangeName("AuthorizationExchange");
            _integrationEventService.EnqueueEvent(identityCreatedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
