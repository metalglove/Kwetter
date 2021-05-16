using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kwetter.Services.NotificationService.API.Application
{
    /// <summary>
    /// Represents the <see cref="WebSocketExtensions"/> class.
    /// Used for mapping endpoints to the websocket middleware.
    /// </summary>
    public static class WebSocketExtensions
    {
        /// <summary>
        /// Adds a websocket handler to the service collection.
        /// </summary>
        /// <typeparam name="TWebSocketHandler">The type of websocket hanbdler.</typeparam>
        /// <param name="app">The app.</param>
        /// <param name="path">The path.</param>
        /// <returns>Returns the application builder to chain further upon.</returns>
        public static IApplicationBuilder MapWebSocketManager<TWebSocketHandler>(this IApplicationBuilder app, PathString path) where TWebSocketHandler : WebSocketHandler
        {
            return app.Map(path, self => self.UseMiddleware<WebSocketManagerMiddleware>(self.ApplicationServices.GetService<TWebSocketHandler>()));
        }
    }
}
