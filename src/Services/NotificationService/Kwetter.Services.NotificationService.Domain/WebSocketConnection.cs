using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.Domain
{
    /// <summary>
    /// Represents the <see cref="WebSocketConnection"/> class.
    /// </summary>
    public sealed class WebSocketConnection : IAsyncDisposable
    {
        private readonly WebSocket _webSocket;

        /// <summary>
        /// Gets the connection id.
        /// </summary>
        public string ConnectionId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketConnection"/> class.
        /// </summary>
        /// <param name="webSocket">The websocket.</param>
        public WebSocketConnection(WebSocket webSocket)
        {
            _webSocket = webSocket;
            ConnectionId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Sends a message to the websocket asynchronously.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable value task.</returns>
        public ValueTask SendMessageAsync(ReadOnlyMemory<byte> message, CancellationToken cancellationToken)
        {
            if (_webSocket.State != WebSocketState.Open)
                return ValueTask.CompletedTask;
            return _webSocket.SendAsync(message, WebSocketMessageType.Text, true, cancellationToken);
        }

        /// <summary>
        /// Closes the connection asynchronously.
        /// </summary>
        /// <param name="closeStatus">The close status.</param>
        /// <param name="statusDescription">The status description.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription)
        {
            if (_webSocket.State == WebSocketState.Open)
                return Task.CompletedTask;
            return _webSocket.CloseAsync(closeStatus, statusDescription, CancellationToken.None);
        }

        /// <summary>
        /// Disposes of the websocket connection asynchronously.
        /// </summary>
        /// <returns>Returns an awaitable value task.</returns>
        public async ValueTask DisposeAsync()
        {
            if (_webSocket is null)
                return;
            if (_webSocket.State != WebSocketState.Open)
            {
                await CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection is disposed normally.");
            }
            _webSocket.Dispose();
        }
    }
}
