using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Kwetter.Services.Common.Infrastructure.RabbitMq
{
    /// <summary>
    /// Represents the <see cref="RabbitConfiguration"/> class.
    /// Automatically declares a dead letter queue for the rabbitmq instance.
    /// </summary>
    public sealed class RabbitConfiguration
    {
        private readonly ObjectPool<IModel> _channelPool;
        private const string DeadLetterExchange = "DeadLetterExchange";
        private const string DeadLetterQueue = "DeadLetterQueue";

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitConfiguration"/> class.
        /// </summary>
        /// <param name="objectPoolingPolicy">The object pooling policy.</param>
        public RabbitConfiguration(IPooledObjectPolicy<IModel> objectPoolingPolicy)
        {
            _channelPool = new DefaultObjectPool<IModel>(objectPoolingPolicy ?? throw new ArgumentNullException(nameof(objectPoolingPolicy)), Environment.ProcessorCount * 2);
            IModel channel = GetChannel();
            channel.ExchangeDeclare(DeadLetterExchange, ExchangeType.Fanout, durable: true);
            channel.QueueDeclare(DeadLetterQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(DeadLetterQueue, DeadLetterExchange, "");
            ReturnChannel(channel);
        }

        /// <summary>
        /// Declares an exchange on the rabbitmq instance.
        /// </summary>
        /// <param name="exchangeName">The exchange name.</param>
        /// <param name="exchangeType">The exchange type.</param>
        public void DeclareExchange(string exchangeName, string exchangeType)
        {
            IModel channel = GetChannel();
            channel.ExchangeDeclare(exchangeName, exchangeType, durable: true);
            ReturnChannel(channel);
        }

        /// <summary>
        /// Declares and binds a queue to an exchange.
        /// </summary>
        /// <param name="exchangeName">The exchange name.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="routingKey">The routing key.</param>
        public void DeclareAndBindQueueToExchange(string exchangeName, string queueName, string routingKey)
        {
            Dictionary<string, object> arguments = new()
            {
                { "x-dead-letter-exchange", DeadLetterExchange }
            };
            IModel channel = GetChannel();
            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: arguments);
            channel.QueueBind(queueName, exchangeName, routingKey);
            ReturnChannel(channel);
        }

        /// <summary>
        /// Gets a channel from the channel pool.
        /// </summary>
        /// <returns>Returns an active channel.</returns>
        public IModel GetChannel()
        {
            return _channelPool.Get();
        }

        /// <summary>
        /// Ensures that the channel is safely returned to the channel pool.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public void ReturnChannel(IModel channel)
        {
            _channelPool.Return(channel);
        }
    }
}
