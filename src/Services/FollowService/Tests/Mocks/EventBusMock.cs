using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.Common.EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Kwetter.Services.FollowService.Tests.Mocks
{
    public class EventBusMock : IEventBus
    {
        private readonly ILogger<EventBusMock> _logger;
        private readonly IMessageSerializer _messageSerializer;
        private readonly ConcurrentBag<IEvent> _unknownEvents;
        private readonly ConcurrentBag<string> _unknownQueue;
        private readonly ConcurrentDictionary<string, Dictionary<string, List<IEventHandler<IEvent>>>> _eventHandlerMap;
        
        public EventBusMock(
            ILogger<EventBusMock> logger,
            IMessageSerializer messageSerializer)
        {
            _logger = logger;
            _messageSerializer = messageSerializer;
            _unknownEvents = new ConcurrentBag<IEvent>();
            _unknownQueue = new ConcurrentBag<string>();
            _eventHandlerMap = new ConcurrentDictionary<string, Dictionary<string, List<IEventHandler<IEvent>>>>();
        }
        
        public void Publish<TEvent>(TEvent @event, string queueName) where TEvent : class, IEvent
        {
            _logger.LogInformation(_messageSerializer.SerializeToString(@event));
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

            foreach (IEventHandler<IEvent> eventHandler in _eventHandlerMap[queueName][nameof(TEvent)])
            {
                Task.Run(() => eventHandler.HandleAsync(@event, CancellationToken.None));
            }
        }

        public void Subscribe<TEvent, TEventHandler>(string queueName, TEventHandler eventHandler) where TEvent : class, IEvent where TEventHandler : IEventHandler<TEvent>
        {
            IEventHandler<IEvent> handler = eventHandler as IEventHandler<IEvent>;
            _eventHandlerMap.AddOrUpdate(
                queueName, 
                new Dictionary<string, List<IEventHandler<IEvent>>>() {{queueName, new List<IEventHandler<IEvent>>
                    {
                        handler
                    }}},
                (key, oldValue) =>
                {
                    if (oldValue.TryGetValue(nameof(TEvent), out List<IEventHandler<IEvent>> eventHandlers))
                    {
                        eventHandler.UnsubscribeEventHandler += (_, _) => eventHandlers.Remove(handler);
                        eventHandlers.Add(handler);
                    }
                    else
                    {
                        List<IEventHandler<IEvent>> eventHandlers2 = new() {handler};
                        eventHandler.UnsubscribeEventHandler += (_, _) => eventHandlers2.Remove(handler);
                        oldValue.Add(nameof(TEvent), eventHandlers2);
                    }
                    return oldValue;
                });
        }

        public void Unsubscribe<TEvent, TEventHandler>(TEventHandler eventHandler) where TEvent : class, IEvent where TEventHandler : IEventHandler<TEvent>
        {
            eventHandler.Unsubscribe();
        }
    }
}