using Kwetter.Services.Common.EventBus.Abstractions;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="EventBus"/> class.
    /// </summary>
    public sealed class EventBus : IEventBus
    {
        private readonly ObjectPool<IModel> _channelPool;
        private readonly IMessageSerializer _messageSerializer;
        private const string Exchange = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBus"/> class.
        /// </summary>
        /// <param name="objectPolicy">The object pooling policy.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        public EventBus(IPooledObjectPolicy<IModel> objectPolicy, IMessageSerializer messageSerializer)
        {
            _channelPool = new DefaultObjectPool<IModel>(objectPolicy ?? throw new ArgumentNullException(nameof(objectPolicy)), Environment.ProcessorCount * 2);
            _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
        }

        /// <inheritdoc cref="IEventPublisher.Publish{TEvent}(TEvent,string)"/>
        public void Publish<TEvent>(TEvent @event, string queueName) where TEvent : class, IEvent
        {
            IModel channel = GetChannel(queueName);
            ReadOnlyMemory<byte> message = _messageSerializer.Serialize(@event);
            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;
            channel.BasicPublish(Exchange, queueName, basicProperties: basicProperties, body: message);
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
                TEvent @event = _messageSerializer.Deserialize<TEvent>(eventArgs.Body);
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
