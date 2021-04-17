using EventStore.ClientAPI;
using Kwetter.Services.Common.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Eventing
{
    /// <summary>
    /// Represents the <see cref="EventStoreConnectionFactory"/> class.
    /// </summary>
    public sealed class EventStoreConnectionFactory : IAsyncFactory<IEventStoreConnection>, IDisposable
    {
        private readonly EventStoreConfiguration _eventStoreConfiguration;
        private readonly ILogger<EventStoreConnectionFactory> _logger;
        private IEventStoreConnection eventStoreConnection;
        private bool isConnected;
        private bool isConnecting;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreConnectionFactory"/> class.
        /// </summary>
        /// <param name="eventStoreConfigurationOptions">The event store configuration options.</param>
        /// <param name="logger">The logger.</param>
        public EventStoreConnectionFactory(
            IOptions<EventStoreConfiguration> eventStoreConfigurationOptions,
            ILogger<EventStoreConnectionFactory> logger)
        {
            _eventStoreConfiguration = eventStoreConfigurationOptions.Value;
            _logger = logger;
        }

        /// <inheritdoc cref="IAsyncFactory{TResult}.CreateAsync(CancellationToken)"/>
        public async Task<IEventStoreConnection> CreateAsync(CancellationToken cancellationToken)
        {
            if (eventStoreConnection is not null)
            {
                if (isConnected)
                    return eventStoreConnection;
                if (isConnecting)
                {
                    return await Retry.DoAsync(() =>
                    {
                        if (isConnecting)
                            throw new Exception("The EventStore is still connecting...");
                        return Task.FromResult(eventStoreConnection);
                    }, maxRetries: 5, cancellationToken: cancellationToken);
                }
            }
            await ConnectAsync(cancellationToken);
            return eventStoreConnection;
        }

        private IEventStoreConnection SetupConnection(CancellationToken cancellationToken)
        {
            IEventStoreConnection localeventStoreConnection = EventStoreConnection.Create(ConnectionSettings.Create().DisableTls().Build(), new Uri(_eventStoreConfiguration.ConnectionUrl));
            localeventStoreConnection.ErrorOccurred += async (s, e) =>
            {
                isConnected = false;
                isConnecting = false;
                _logger.LogError($"Error occurred on the EventStore connection: {e.Exception.Message}\n Attempting to reconnect...");
                await ConnectAsync(cancellationToken);
            };
            localeventStoreConnection.Disconnected += async (s, e) =>
            {
                isConnected = false;
                isConnecting = false;
                _logger.LogError($"The EventStore connection disconnected. Attempting to reconnect...");
                await ConnectAsync(cancellationToken);
            };
            localeventStoreConnection.Closed += async (s, e) =>
            {
                isConnected = false;
                isConnecting = false;
                _logger.LogError($"The EventStore connection closed. Attempting to create a new connection...");
                await ConnectAsync(cancellationToken);
            };
            return localeventStoreConnection;
        }

        private async Task ConnectAsync(CancellationToken cancellationToken)
        {
            eventStoreConnection = SetupConnection(cancellationToken);
            isConnecting = true;
            await eventStoreConnection.ConnectAsync();
            isConnected = true;
            isConnecting = false;
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            eventStoreConnection?.Dispose();
        }
    }
}
