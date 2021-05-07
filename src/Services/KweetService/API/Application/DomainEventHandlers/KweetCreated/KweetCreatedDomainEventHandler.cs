using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.DomainEventHandlers.KweetCreated
{
    /// <summary>
    /// Represents the <see cref="KweetCreatedDomainEventHandler"/> class.
    /// </summary>
    public sealed class KweetCreatedDomainEventHandler : INotificationHandler<KweetCreatedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetCreatedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public KweetCreatedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the kweet created event and enqueues the kweet created integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(KweetCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            KweetCreatedIntegrationEvent kweetCreatedIntegrationEvent = new(notification.KweetId, notification.UserId, notification.HashTags, notification.Mentions, notification.Message, notification.CreatedDateTime);
            kweetCreatedIntegrationEvent.SetExchangeName("KweetExchange");
            kweetCreatedIntegrationEvent.SetRoutingKey("KweetService.KweetCreatedIntegrationEvent");
            _integrationEventService.EnqueueEvent(kweetCreatedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
