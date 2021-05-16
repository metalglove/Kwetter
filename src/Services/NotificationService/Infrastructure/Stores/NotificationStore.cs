using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.Infrastructure.Stores
{
    /// <summary>
    /// Represents the <see cref="NotificationStore"/> class.
    /// </summary>
    public sealed class NotificationStore : INotificationStore
    {
        private readonly ILogger<NotificationStore> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        private IDatabase Database => _connectionMultiplexer.GetDatabase(0);

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationStore"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="connectionMultiplexer">The connection multiplexer.</param>
        public NotificationStore(ILogger<NotificationStore> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
        }

        /// <inheritdoc cref="INotificationStore.AppendNotificationAsync(string, string, CancellationToken)"/>
        public async Task<bool> AppendNotificationAsync(string userId, string notification, CancellationToken cancellationToken)
        {
            try
            {
                await Database.ListRightPushAsync($"user:notifications:{userId}", notification);
                return true;
            }
            catch (Exception)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return false;
            }
        }

        /// <inheritdoc cref="INotificationStore.GetNotificationsAsync(string, CancellationToken)"/>
        public async IAsyncEnumerable<string> GetNotificationsAsync(string userId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            string key = $"user:notifications:{userId}";
            long length = await Database.ListLengthAsync(key);
            if (length == 0)
                yield break;
            cancellationToken.ThrowIfCancellationRequested();
            string notification;
            do
            {
                notification = await Database.ListLeftPopAsync(key);
                if (notification is null)
                    yield break;
                yield return notification;
                cancellationToken.ThrowIfCancellationRequested();
            }
            while (true);
        }

        /// <inheritdoc cref="INotificationStore.PrependNotificationsAsync(string, IEnumerable{string}, CancellationToken)"/>
        public async Task<bool> PrependNotificationsAsync(string userId, IEnumerable<string> notifications, CancellationToken cancellationToken)
        {
            try
            {
                await Database.ListLeftPushAsync($"user:notifications:{userId}", notifications.Reverse().Select(n => new RedisValue(n)).ToArray());
                return true;
            }
            catch (Exception)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return false;
            }
        }

        /// <inheritdoc cref="INotificationStore.PublishNotificationAsync(string, string)"/>
        public async Task PublishNotificationAsync(string userId, string message)
        {
            await Database.PublishAsync($"channel:user:notifications:{userId}", message);
        }

        /// <inheritdoc cref="INotificationStore.SubscribeNotificationAsync(string, Func{string, ValueTask})"/>
        public async Task<Action> SubscribeNotificationAsync(string userId, Func<string, ValueTask> handler)
        {
            ISubscriber subscriber = _connectionMultiplexer.GetSubscriber();
            string channel = $"channel:user:notifications:{userId}";
            async void RedisMessageHandler(ChannelMessage message)
            {
                if (message.Message.IsNullOrEmpty)
                {
                    _logger.LogWarning($"Message received by [{channel}] was null or empty.");
                    return;
                }
                await handler(message.Message);
            }
            ChannelMessageQueue channelMessageQueue = await subscriber.SubscribeAsync(channel);
            channelMessageQueue.OnMessage(RedisMessageHandler);
            void Unsubscribe() => channelMessageQueue.Unsubscribe();
            return Unsubscribe;
        }
    }
}
