using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Domain.Events;
using Kwetter.Services.Common.Infrastructure.RabbitMq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Kwetter.Services.Common.Infrastructure.Eventing.Bus
{
    /// <summary>
    /// Represents the <see cref="EventBus"/> class.
    /// </summary>
    public sealed class EventBus : IEventBus
    {
        private readonly ILogger<EventBus> _logger;
        private readonly RabbitConfiguration _rabbitConfiguration;
        private readonly IEventSerializer _eventSerializer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Dictionary<string, Action> _unsubscribeDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBus"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="rabbitConfiguration">The rabbit configuration.</param>
        /// <param name="eventSerializer">The event serializer.</param>
        /// <param name="serviceScopeFactory">The service scope factory.</param>
        public EventBus(
            ILogger<EventBus> logger,
            RabbitConfiguration rabbitConfiguration, 
            IEventSerializer eventSerializer,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rabbitConfiguration = rabbitConfiguration ?? throw new ArgumentNullException(nameof(rabbitConfiguration));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _unsubscribeDictionary = new Dictionary<string, Action>();
        }

        /// <inheritdoc cref="IEventPublisher.Publish{TEvent}(TEvent, string, string)"/>
        public void Publish<TEvent>(TEvent @event, string exchangeName, string routingKey) where TEvent : Event
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            if (routingKey is null)
                throw new ArgumentNullException(nameof(routingKey));
            if (string.IsNullOrWhiteSpace(exchangeName))
                throw new ArgumentNullException(nameof(exchangeName));

            IModel channel = _rabbitConfiguration.GetChannel();
            // NOTE: type casted to object to ensure no type issues when serializing.
            ReadOnlyMemory<byte> message = _eventSerializer.Serialize<object>(@event);
            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;
            channel.BasicPublish(exchangeName, routingKey: routingKey, basicProperties: basicProperties, body: message);
            _rabbitConfiguration.ReturnChannel(channel);
        }

        /// <inheritdoc cref="IEventBus.Subscribe{TEvent,TEventHandler}(string)"/>
        public void Subscribe<TEvent, TEventHandler>(string queueName) 
            where TEvent : Event 
            where TEventHandler : IEventHandler<TEvent>
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentNullException(nameof(queueName));

            IModel channel = _rabbitConfiguration.GetChannel();
            AsyncEventingBasicConsumer eventingBasicConsumer = new(channel);
            eventingBasicConsumer.Received += async (sender, eventArgs) =>
            {
                try
                {
                    TEvent @event = _eventSerializer.Deserialize<TEvent>(eventArgs.Body);
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        CommandResponse response = await mediator.Send(@event, CancellationToken.None).ConfigureAwait(true) as CommandResponse;
                        if (response.Success)
                            channel.BasicAck(eventArgs.DeliveryTag, false);
                        else
                            channel.BasicNack(eventArgs.DeliveryTag, false, false);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is InvalidOperationException)
                    {
                        _logger.LogError($"Queue: [{queueName}]-[{typeof(TEvent).Name}] failed to deserialize.\n{ex.Message}");
                    }
                    channel.BasicNack(eventArgs.DeliveryTag, false, false);
                }
            };
            string tag = channel.BasicConsume(queueName, false, eventingBasicConsumer);
            _unsubscribeDictionary.Add(queueName, () => channel.BasicCancel(tag));
            _rabbitConfiguration.ReturnChannel(channel);
        }

        /// <inheritdoc cref="IEventBus.Unsubscribe(string)"/>
        public void Unsubscribe(string queueName)
        {
            if (_unsubscribeDictionary.TryGetValue(queueName, out Action unsubscribeAction))
            {
                unsubscribeAction();
            }
        }
    }
}
