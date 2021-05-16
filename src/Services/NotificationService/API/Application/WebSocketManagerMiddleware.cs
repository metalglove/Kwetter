using Kwetter.Services.Common.Application.Dtos;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Interfaces;
using Kwetter.Services.NotificationService.API.Application.Handlers.Events;
using Microsoft.AspNetCore.Http;
using System;
using System.Buffers;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.API.Application
{
    /// <summary>
    /// Represents the <see cref="WebSocketManagerMiddleware"/> class.
    /// This middleware provides websocket functionality for the notification service with authorization.
    /// </summary>
    public sealed class WebSocketManagerMiddleware
    {
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ITokenVerifier _tokenVerifier;
        private readonly IEventSerializer _eventSerialier;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketManagerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next request delegate.</param>
        /// <param name="webSocketHandler">The websocket handler.</param>
        /// <param name="tokenVerifier">The token verifier.</param>
        /// <param name="eventSerializer">The event serializer.</param>
        public WebSocketManagerMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler, ITokenVerifier tokenVerifier, IEventSerializer eventSerializer)
        {
            _ = next; // NOTE: Ignore middleware continuation, since it is a websocket.
            _webSocketHandler = webSocketHandler;
            _tokenVerifier = tokenVerifier;
            _eventSerialier = eventSerializer;
        }

        /// <summary>
        /// Invokes the middleware asynchronously.
        /// </summary>
        /// <param name="context">The http context of the request.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async Task Invoke(HttpContext context)
        {
            // Ensure that this request is a websocket request.
            if (!context.WebSockets.IsWebSocketRequest)
                return;
            
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

            // NOTE: first message is used for auth due to limitation in websocket api.
            // https://docs.microsoft.com/nl-nl/aspnet/core/signalr/security?view=aspnetcore-5.0#access-token-logging
            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);

            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;
            string userId;
            try
            {
                // Verify user token and find user id for connection.
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                string token = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                ClaimsDto claims = await _tokenVerifier.VerifyIdTokenAsync(token, cancellationToken);
                userId = claims.Claims.Single(claim => claim.Key == "UserId").Value.Value;
            }
            catch (Exception)
            {
                ReadOnlyMemory<byte> message = _eventSerialier.Serialize(new UserUnauthorizedEvent()
                {
                    Message = "User is not authorised."
                });
                await webSocket.SendAsync(message, WebSocketMessageType.Text, true, cancellationToken);
                return;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }

            // Handle the user identifiable websocket connection.
            await _webSocketHandler.HandleAsync(userId, webSocket, cancellationToken);
        }
    }
}
