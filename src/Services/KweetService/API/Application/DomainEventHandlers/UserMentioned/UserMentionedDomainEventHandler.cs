using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.DomainEventHandlers.UserMentioned
{
    /// <summary>
    /// Represents the <see cref="UserMentionedDomainEventHandler"/> class.
    /// </summary>
    public sealed class UserMentionedDomainEventHandler : INotificationHandler<UserMentionedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMentionedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public UserMentionedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the user mentioned event and enqueues the user mentioned integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(UserMentionedDomainEvent notification, CancellationToken cancellationToken)
        {
            UserMentionedIntegrationEvent userMentionedIntegrationEvent = new(notification.KweetId, notification.UserId, notification.UserName, notification.MentionedDateTime);
            userMentionedIntegrationEvent.SetExchangeName("KweetExchange");
            userMentionedIntegrationEvent.SetRoutingKey("KweetService.UserMentionedIntegrationEvent");
            _integrationEventService.EnqueueEvent(userMentionedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
