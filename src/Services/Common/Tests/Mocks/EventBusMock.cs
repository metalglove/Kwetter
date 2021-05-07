using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.Common.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Kwetter.Services.Common.Tests.Mocks
{
    // TODO: IGNORES EXCHANGE CURRENTLY!
    public class EventBusMock : IEventBus
    {
        private readonly ILogger<EventBusMock> _logger;
        private readonly IEventSerializer _eventSerializer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConcurrentBag<Event> _unknownEvents;
        private readonly ConcurrentBag<string> _unknownQueue;
        private readonly ConcurrentDictionary<string, Dictionary<string, List<IEventHandler<Event>>>> _eventHandlerMap;

        public EventBusMock(
            ILogger<EventBusMock> logger,
            IEventSerializer messageSerializer,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _eventSerializer = messageSerializer;
            _serviceScopeFactory = serviceScopeFactory;
            _unknownEvents = new ConcurrentBag<Event>();
            _unknownQueue = new ConcurrentBag<string>();
            _eventHandlerMap = new ConcurrentDictionary<string, Dictionary<string, List<IEventHandler<Event>>>>();
        }

        public void Publish<TEvent>(TEvent @event, string exchangeName, string routingKey) where TEvent : Event
        {
            // TODO: mock exchange Publish stuff
            _logger.LogInformation(_eventSerializer.SerializeToString(@event));
            using var scope = _serviceScopeFactory.CreateScope();
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            if (@event is IncomingIntegrationEvent)
                mediator.Send(@event, CancellationToken.None).GetAwaiter().GetResult();
        }

        public void Subscribe<TEvent, TEventHandler>(string queueName) 
            where TEvent : Event 
            where TEventHandler : IEventHandler<TEvent>
        {
            // TODO: sub
        }

        public void Unsubscribe(string queueName) 
        {
            // TODO: unsub
        }
    }
}