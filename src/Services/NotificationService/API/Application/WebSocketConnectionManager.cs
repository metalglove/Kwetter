using Kwetter.Services.NotificationService.Domain;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kwetter.Services.NotificationService.API.Application
{
    /// <summary>
    /// Represents the <see cref="WebSocketConnectionManager"/> class.
    /// </summary>
    public sealed class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, UserConnection> _userConnections = new();

        /// <summary>
        /// Gets a read only dictionary of user connections.
        /// </summary>
        public IReadOnlyDictionary<string, UserConnection> UserConnections => _userConnections;

        /// <summary>
        /// Gets the user connection for a specific user id if found; otherwise, default.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns the user connection if found; otherwise, default.</returns>
        public UserConnection GetUserConnectionOrDefault(string userId)
        {
            if (_userConnections.ContainsKey(userId))
                return _userConnections[userId];
            return default;
        }

        /// <summary>
        /// Creates a new user connection or adds a connection to an existing user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="webSocketConnection">The websocket connection.</param>
        /// <returns>Returns a boolean indicating whether the user connection is created or added to an existing user.</returns>
        public bool CreateOrAddConnection(string userId, WebSocketConnection webSocketConnection)
        {
            UserConnection userConnection = GetUserConnectionOrDefault(userId);
            if (userConnection == default)
            {
                userConnection = new UserConnection(userId, webSocketConnection);
                return _userConnections.TryAdd(userId, userConnection);
            }
            userConnection.AddConnection(webSocketConnection);
            return true;
        }

        /// <summary>
        /// Removes a websocket connection from a user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="webSocketConnection">The websocket connection.</param>
        /// <returns>Returns a boolean indicating whether the websocket connection is removed.</returns>
        public bool RemoveConnection(string userId, WebSocketConnection webSocketConnection)
        {
            return _userConnections[userId].RemoveConnection(webSocketConnection);
        }
    }
}
