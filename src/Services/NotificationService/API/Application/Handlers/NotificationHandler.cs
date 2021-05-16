using Kwetter.Services.NotificationService.Domain;
using Kwetter.Services.NotificationService.Infrastructure.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.API.Application.Handlers
{
    /// <summary>
    /// Represents the <see cref="NotificationHandler"/> class.
    /// </summary>
    public class NotificationHandler : WebSocketHandler, INotificationPublisher
    {
        private const int ReadBufferLength = 4096;
        private readonly INotificationStore _notificationStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationHandler"/> class.
        /// </summary>
        /// <param name="notificationStore">The notification store.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="webSocketConnectionManager">The web socket connection manager.</param>
        public NotificationHandler(
            INotificationStore notificationStore,
            ILogger<NotificationHandler> logger,
            WebSocketConnectionManager webSocketConnectionManager) : base(logger, webSocketConnectionManager)
        {
            _notificationStore = notificationStore ?? throw new ArgumentNullException(nameof(notificationStore));
        }

        /// <inheritdoc cref="WebSocketHandler.HandleAsync(string, WebSocket, CancellationToken)"/>
        public async override Task HandleAsync(string userId, WebSocket webSocket, CancellationToken cancellationToken)
        {
            WebSocketConnection webSocketConnection = new(webSocket);
            OnConnected(userId, webSocketConnection);

            Action unsubscribe = await _notificationStore.SubscribeNotificationAsync(userId, 
                (message) => webSocketConnection.SendMessageAsync(Encoding.UTF8.GetBytes(message), cancellationToken));
            
            byte[] buffer = ArrayPool<byte>.Shared.Rent(ReadBufferLength);
            try
            {
                while (webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    // Maintain the connection. 
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    if (result.MessageType == WebSocketMessageType.Close)
                        OnDisconnected(userId, webSocketConnection);
                    Array.Clear(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {
                Logger.LogInformation($"User {userId} connection abruptly closed.");
            }
            finally
            {
                unsubscribe();
                OnDisconnected(userId, webSocketConnection);
                await webSocketConnection.DisposeAsync();
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        /// <inheritdoc cref="INotificationPublisher.PublishNotificationAsync(string, ReadOnlyMemory{byte}, CancellationToken)"/>
        public Task<bool> PublishNotificationAsync(string userId, ReadOnlyMemory<byte> message, CancellationToken cancellationToken)
        {
            UserConnection userConnection = GetUserConnection(userId);
            if (userConnection == default)
                return Task.FromResult(false);
            return userConnection.PublishMessageAsync(message, cancellationToken);
        }
    }
}
