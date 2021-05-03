using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Domain.Events;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Tests.Mocks
{
    // TODO: IGNORES EXCHANGE CURRENTLY!
    public class EventBusMock : IEventBus
    {
        private readonly ILogger<EventBusMock> _logger;
        private readonly IEventSerializer _eventSerializer;
        private readonly ConcurrentBag<Event> _unknownEvents;
        private readonly ConcurrentBag<string> _unknownQueue;
        private readonly ConcurrentDictionary<string, Dictionary<string, List<IEventHandler<Event>>>> _eventHandlerMap;
        
        public EventBusMock(
            ILogger<EventBusMock> logger,
            IEventSerializer messageSerializer)
        {
            _logger = logger;
            _eventSerializer = messageSerializer;
            _unknownEvents = new ConcurrentBag<Event>();
            _unknownQueue = new ConcurrentBag<string>();
            _eventHandlerMap = new ConcurrentDictionary<string, Dictionary<string, List<IEventHandler<Event>>>>();
        }

        public void DeclareExchange(string exchangeName, ExchangeType exchangeType)
        {
            // TODO: declare exchange mock
            _logger.LogWarning($"DeclareExchange({exchangeName}) called on EventBusMock; Exchanges are currently ignored!");
        }

        public void Publish<TEvent>(TEvent @event, string exchangeName, string queueName) where TEvent : Event
        {
            // TODO: mock exchange Publish stuff
            _logger.LogInformation(_eventSerializer.SerializeToString(@event));
            if (!_eventHandlerMap.ContainsKey(nameof(TEvent)))
            {
                _unknownQueue.Add(queueName);
                _unknownEvents.Add(@event);
                return;
            }
            if (!_eventHandlerMap.ContainsKey(nameof(TEvent)))
            {
                _unknownEvents.Add(@event);
                return;
            }

            foreach (IEventHandler<Event> eventHandler in _eventHandlerMap[queueName][nameof(TEvent)])
            {
                Task.Run(() => eventHandler.HandleAsync(@event, CancellationToken.None));
            }
        }

        public void Subscribe<TEvent, TEventHandler>(string exchangeName, string queueName, TEventHandler eventHandler) 
            where TEvent : Event 
            where TEventHandler : IEventHandler<TEvent>
        {
            // TODO: mock exchange Subscribe stuff
            IEventHandler<Event> handler = eventHandler as IEventHandler<Event>;
            _eventHandlerMap.AddOrUpdate(
                queueName, 
                new Dictionary<string, List<IEventHandler<Event>>>() {{queueName, new List<IEventHandler<Event>>
                    {
                        handler
                    }}},
                (key, oldValue) =>
                {
                    if (oldValue.TryGetValue(nameof(TEvent), out List<IEventHandler<Event>> eventHandlers))
                    {
                        eventHandler.UnsubscribeEventHandler += (_, _) => eventHandlers.Remove(handler);
                        eventHandlers.Add(handler);
                    }
                    else
                    {
                        List<IEventHandler<Event>> eventHandlers2 = new() {handler};
                        eventHandler.UnsubscribeEventHandler += (_, _) => eventHandlers2.Remove(handler);
                        oldValue.Add(nameof(TEvent), eventHandlers2);
                    }
                    return oldValue;
                });
        }

        public void Unsubscribe<TEvent, TEventHandler>(TEventHandler eventHandler) 
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent>
        {
            eventHandler.Unsubscribe();
        }
    }
}