using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Infrastructure.RabbitMq;
using Kwetter.Services.NotificationService.API.Application;
using Kwetter.Services.NotificationService.API.Application.Handlers;
using Kwetter.Services.NotificationService.API.Application.IntegrationEventHandlers.KweetLiked;
using Kwetter.Services.NotificationService.API.Application.IntegrationEventHandlers.UserFollowed;
using Kwetter.Services.NotificationService.API.Application.IntegrationEventHandlers.UserMentioned;
using Kwetter.Services.NotificationService.Infrastructure.Stores;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Kwetter.Services.NotificationService.API
{
    /// <summary>
    /// Represents the <see cref="Startup"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Configures the services in the service collection container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurations(_configuration);
            services.AddDefaultAuthentication(_configuration);
            services.AddMediatR(Assembly.GetAssembly(typeof(Startup)));
            services.AddDefaultInfrastructureServices();
            services.AddRedis(_configuration);
            services.AddTransient<INotificationStore, NotificationStore>();
            services.AddTransient<WebSocketConnectionManager>();
            services.AddSingleton<NotificationHandler>();
            services.AddSingleton<INotificationPublisher>(builder => builder.GetService<NotificationHandler>());

            services.AddIntegrationEventHandler<KweetLikedIntegrationEventHandler, KweetLikedIntegrationEvent>();
            services.AddIntegrationEventHandler<UserMentionedIntegrationEventHandler, UserMentionedIntegrationEvent>();
            services.AddIntegrationEventHandler<UserFollowedIntegrationEventHandler, UserFollowedIntegrationEvent>();

            services.AddControllers();
            services.AddSwagger(_configuration);
        }

        /// <summary>
        /// Configures the application request response flow (middleware).
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web host environment.</param>
        /// <param name="eventBus">The event bus.</param>        
        /// <param name="rabbitConfiguration">The rabbit configuration.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEventBus eventBus, RabbitConfiguration rabbitConfiguration)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                string version = _configuration["Service:Version"];
                string title = _configuration["Service:Title"];
                app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{title} {version}"));
            }

            // Declare used exchanges!
            rabbitConfiguration.DeclareExchange("KweetExchange", ExchangeType.Topic);
            rabbitConfiguration.DeclareExchange("FollowExchange", ExchangeType.Topic);
            rabbitConfiguration.DeclareAndBindQueueToExchange("KweetExchange", "NotificationService.KweetLikedIntegrationEvent", "#.KweetLikedIntegrationEvent");
            rabbitConfiguration.DeclareAndBindQueueToExchange("KweetExchange", "NotificationService.UserMentionedIntegrationEvent", "#.UserMentionedIntegrationEvent");
            rabbitConfiguration.DeclareAndBindQueueToExchange("FollowExchange", "NotificationService.UserFollowedIntegrationEvent", "#.UserFollowedIntegrationEvent");

            // Subscribe to integration events.
            eventBus.Subscribe<KweetLikedIntegrationEvent, KweetLikedIntegrationEventHandler>("NotificationService.KweetLikedIntegrationEvent");
            eventBus.Subscribe<UserMentionedIntegrationEvent, UserMentionedIntegrationEventHandler>("NotificationService.UserMentionedIntegrationEvent");
            eventBus.Subscribe<UserFollowedIntegrationEvent, UserFollowedIntegrationEventHandler>("NotificationService.UserFollowedIntegrationEvent");

            app.UseWebSockets();
            app.MapWebSocketManager<NotificationHandler>("/api/Notification/Live");

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
