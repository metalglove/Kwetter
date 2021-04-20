﻿using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Application.Eventing.Integration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventService"/> class.
    /// </summary>
    public sealed class IntegrationEventService : IIntegrationEventService
    {
        private readonly ConcurrentQueue<IntegrationEvent> _integrationEvents;
        private readonly ILogger<IntegrationEventService> _logger;
        private readonly IEventBus _eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="eventBus">The event bus.</param>
        public IntegrationEventService(
            ILogger<IntegrationEventService> logger,
            IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEvents = new ConcurrentQueue<IntegrationEvent>();
        }

        /// <inheritdoc cref="IIntegrationEventService.PublishEvents()"/>
        public void PublishEvents()
        {
            if (_integrationEvents.IsEmpty)
            {
                _logger.LogInformation($"No integration events to publish.");
                return;
            }

            while (!_integrationEvents.IsEmpty)
            {
                _integrationEvents.TryDequeue(out IntegrationEvent @event);
                _eventBus.Publish(@event, $"Integration.{@event.EventName}");
                _logger.LogInformation($"Dequeued & published integration event: {@event.EventName}:{@event.EventId}");
            }
        }

        /// <inheritdoc cref="IIntegrationEventService.EnqueueEvent{TIntegrationEvent}(TIntegrationEvent)"/>
        public void EnqueueEvent<TIntegrationEvent>(TIntegrationEvent @event) where TIntegrationEvent : IntegrationEvent
        {
            _integrationEvents.Enqueue(@event);
            _logger.LogInformation($"Enqueued integration event: {@event.EventName}:{@event.EventId}");
        }
    }
}
