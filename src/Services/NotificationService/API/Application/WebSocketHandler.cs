using Kwetter.Services.NotificationService.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.API.Application
{
    /// <summary>
    /// Represents the <see cref="WebSocketHandler"/> class.
    /// </summary>
    public abstract class WebSocketHandler
    {
        private readonly WebSocketConnectionManager _webSocketConnectionManager;

        /// <summary>
        /// Gets the logger for the websocket handler.
        /// </summary>
        protected readonly ILogger<WebSocketHandler> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="webSocketConnectionManager">The websocket connection manager.</param>
        protected WebSocketHandler(ILogger<WebSocketHandler> logger, WebSocketConnectionManager webSocketConnectionManager)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _webSocketConnectionManager = webSocketConnectionManager ?? throw new ArgumentNullException(nameof(webSocketConnectionManager));
        }

        /// <summary>
        /// Handles the websocket request asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="webSocket">The websocket.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public abstract Task HandleAsync(string userId, WebSocket webSocket, CancellationToken cancellationToken);

        /// <inheritdoc cref="WebSocketConnectionManager.GetUserConnectionOrDefault(string)"/>
        protected UserConnection GetUserConnection(string userId) => _webSocketConnectionManager.GetUserConnectionOrDefault(userId);

        /// <summary>
        /// The on connected event for adding the connection to the websocket connection manager.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="webSocketConnection">The websocket connection.</param>
        protected void OnConnected(string userId, WebSocketConnection webSocketConnection)
        {
            _webSocketConnectionManager.CreateOrAddConnection(userId, webSocketConnection);
            Logger.LogInformation($"User {userId} connected.");
        }

        /// <summary>
        /// The on disconnected event for removing the connection from the websocket connection manager.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="webSocketConnection">The websocket connection.</param>
        protected void OnDisconnected(string userId, WebSocketConnection webSocketConnection)
        {
            if (_webSocketConnectionManager.RemoveConnection(userId, webSocketConnection))
                Logger.LogInformation($"User {userId} disconnected.");
        }
    }
}
