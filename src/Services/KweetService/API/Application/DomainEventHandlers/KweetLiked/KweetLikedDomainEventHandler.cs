using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.DomainEventHandlers.KweetLiked
{
    /// <summary>
    /// Represents the <see cref="KweetLikedDomainEventHandler"/> class.
    /// </summary>
    public sealed class KweetLikedDomainEventHandler : INotificationHandler<KweetLikedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetLikedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public KweetLikedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the kweet liked event and enqueues the kweet liked integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(KweetLikedDomainEvent notification, CancellationToken cancellationToken)
        {
            KweetLikedIntegrationEvent kweetLikedIntegrationEvent = new(notification.KweetId, notification.KweetUserId, notification.UserId, notification.LikedDateTime);
            kweetLikedIntegrationEvent.SetExchangeName("KweetExchange");
            kweetLikedIntegrationEvent.SetRoutingKey("KweetService.KweetLikedIntegrationEvent");
            _integrationEventService.EnqueueEvent(kweetLikedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
