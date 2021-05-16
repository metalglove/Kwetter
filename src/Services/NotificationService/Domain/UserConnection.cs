using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.Domain
{
    /// <summary>
    /// Represents the <see cref="UserConnection"/> class.
    /// </summary>
    public sealed class UserConnection : IEquatable<UserConnection>
    {
        private readonly List<WebSocketConnection> _connections;

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets a read onyl list of websocket connections asociated with this user.
        /// </summary>
        public IReadOnlyList<WebSocketConnection> Connections => _connections.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserConnection"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="webSocketConnection">The websocket connection.</param>
        public UserConnection(string userId, WebSocketConnection webSocketConnection)
        {
            UserId = userId;
            _connections = new List<WebSocketConnection>() { webSocketConnection };
        }

        /// <summary>
        /// Equals the other connection by the user id.
        /// </summary>
        /// <param name="other">The other user connection.</param>
        /// <returns>Returns a boolean indicating whether the user connections are equal.</returns>
        public bool Equals(UserConnection other)
        {
            return other.UserId == UserId;
        }

        /// <summary>
        /// Adds a websocket connection to the list of websocket connections.
        /// </summary>
        /// <param name="webSocketConnection">The websocket connection.</param>
        public void AddConnection(WebSocketConnection webSocketConnection)
        {
            _connections.Add(webSocketConnection);
        }

        /// <summary>
        /// Removes a websocket connection from the list of websocket connections.
        /// </summary>
        /// <param name="webSocketConnection">The websocket connection.</param>
        /// <returns>Returns a boolean indicating whether the websocket connection is removed.</returns>
        public bool RemoveConnection(WebSocketConnection webSocketConnection)
        {
            return _connections.Remove(webSocketConnection);
        }

        /// <summary>
        /// Publishes a message to each of the websocket connections of the user.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns true if one or more messages are sent; otherwise, false.</returns>
        public async Task<bool> PublishMessageAsync(ReadOnlyMemory<byte> message, CancellationToken cancellationToken)
        {
            int messagesSent = 0;
            foreach (WebSocketConnection webSocketConnection in _connections)
            {
                await webSocketConnection.SendMessageAsync(message, cancellationToken).ConfigureAwait(false);
                messagesSent += 1;
            }
            return messagesSent >= 1;
        }
    }
}
