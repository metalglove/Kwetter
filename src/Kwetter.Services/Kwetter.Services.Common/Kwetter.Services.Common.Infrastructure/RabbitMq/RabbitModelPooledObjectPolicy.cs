using Kwetter.Services.Common.EventBus;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Kwetter.Services.Common.Infrastructure.RabbitMq
{
    /// <summary>
    /// Represents the <see cref="RabbitModelPooledObjectPolicy"/> class.
    /// </summary>
    public sealed class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly MessagingConfiguration _options;
        private readonly IConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitModelPooledObjectPolicy"/> class.
        /// </summary>
        /// <param name="options"></param>
        public RabbitModelPooledObjectPolicy(IOptions<MessagingConfiguration> options)
        {
            _options = options.Value;
            _connection = GetConnection();
        }

        /// <inheritdoc cref="IPooledObjectPolicy{T}.Create()"/>
        public IModel Create()
        {
            return _connection.CreateModel();
        }

        /// <inheritdoc cref="IPooledObjectPolicy{T}.Return(T)"/>
        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }

            obj?.Dispose();
            return false;
        }

        private IConnection GetConnection()
        {
            ConnectionFactory factory = new()
            {
                DispatchConsumersAsync = true,
                HostName = _options.Host,
                UserName = _options.Username,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VirtualHost
            };

            return factory.CreateConnection();
        }
    }
}
