using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Domain.Events;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace Kwetter.Services.Common.Infrastructure.Eventing.Bus
{
    /// <summary>
    /// Represents the <see cref="EventBus"/> class.
    /// </summary>
    public sealed class EventBus : IEventBus
    {
        private readonly ObjectPool<IModel> _channelPool;
        private readonly ServiceConfiguration _serviceConfiguration;
        private readonly IEventSerializer _eventSerializer;
        private const string Exchange = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBus"/> class.
        /// </summary>
        /// <param name="serviceConfigurationOptions">The service configuration options.</param>
        /// <param name="objectPolicy">The object pooling policy.</param>
        /// <param name="eventSerializer">The event serializer.</param>
        public EventBus(
            IOptions<ServiceConfiguration> serviceConfigurationOptions,
            IPooledObjectPolicy<IModel> objectPolicy, 
            IEventSerializer eventSerializer)
        {
            _channelPool = new DefaultObjectPool<IModel>(objectPolicy ?? throw new ArgumentNullException(nameof(objectPolicy)), Environment.ProcessorCount * 2);
            _serviceConfiguration = serviceConfigurationOptions.Value ?? throw new ArgumentNullException(nameof(serviceConfigurationOptions));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
        }

        /// <inheritdoc cref="IEventPublisher.Publish{TEvent}(TEvent,string)"/>
        public void Publish<TEvent>(TEvent @event, string queueName) where TEvent : class, IEvent
        {
            string actualQueueName = $"{_serviceConfiguration.ShortTitle}.{queueName}";
            IModel channel = GetChannel(actualQueueName);
            ReadOnlyMemory<byte> message = _eventSerializer.Serialize(@event);
            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;
            channel.BasicPublish(Exchange, actualQueueName, basicProperties: basicProperties, body: message);
            _channelPool.Return(channel);
        }

        /// <inheritdoc cref="IEventBus.Subscribe{TEvent,TEventHandler}(string,TEventHandler)"/>
        public void Subscribe<TEvent, TEventHandler>(string queueName, TEventHandler eventHandler)
            where TEvent : class, IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            IModel channel = GetChannel(queueName);
            AsyncEventingBasicConsumer eventingBasicConsumer = new(channel);
            eventingBasicConsumer.Received += async (sender, eventArgs) =>
            {
                TEvent @event = _eventSerializer.Deserialize<TEvent>(eventArgs.Body);
                await eventHandler.HandleAsync(@event, CancellationToken.None);
            };
            string tag = channel.BasicConsume(queueName, true, eventingBasicConsumer);
            eventHandler.UnsubscribeEventHandler += (sender, eventArgs) =>
            {
                channel.BasicCancel(tag);
            };
            _channelPool.Return(channel);
        }

        /// <inheritdoc cref="IEventBus.Unsubscribe{TEvent,TEventHandler}(TEventHandler)"/>
        public void Unsubscribe<TEvent, TEventHandler>(TEventHandler eventHandler)
            where TEvent : class, IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            eventHandler.Unsubscribe();
        }

        private IModel GetChannel(string queueName)
        {
            IModel channel = _channelPool.Get();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            return channel;
        }
    }
}
