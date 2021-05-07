using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.DomainEventHandlers.KweetUnliked
{
    /// <summary>
    /// Represents the <see cref="KweetUnlikedDomainEventHandler"/> class.
    /// </summary>
    public sealed class KweetUnlikedDomainEventHandler : INotificationHandler<KweetUnlikedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetUnlikedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        public KweetUnlikedDomainEventHandler(IIntegrationEventService integrationEventService)
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        /// <summary>
        /// Handles the kweet unliked event and enqueues the kweet unliked integration event.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task Handle(KweetUnlikedDomainEvent notification, CancellationToken cancellationToken)
        {
            KweetUnlikedIntegrationEvent kweetUnlikedIntegrationEvent = new(notification.KweetId, notification.UserId, notification.UnlikedDateTime);
            kweetUnlikedIntegrationEvent.SetExchangeName("KweetExchange");
            kweetUnlikedIntegrationEvent.SetRoutingKey("KweetService.KweetUnlikedIntegrationEvent");
            _integrationEventService.EnqueueEvent(kweetUnlikedIntegrationEvent);
            return Task.CompletedTask;
        }
    }
}
