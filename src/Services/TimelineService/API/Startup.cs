using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Infrastructure.RabbitMq;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.KweetCreated;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.KweetLiked;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.KweetUnliked;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserCreated;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserDisplayNameUpdated;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserFollowed;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserProfileDescriptionUpdated;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserProfilePictureUrlUpdated;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserUnfollowed;
using Kwetter.Services.TimelineService.API.Application.Queries.KweetTimelineQuery;
using Kwetter.Services.TimelineService.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Kwetter.Services.TimelineService.API
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
            services.AddLogging(p => p.AddConsole());
            services.AddDefaultApplicationServices(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(KweetTimelineQuery)));
            services.AddDefaultInfrastructureServices();
            services.AddNeo4jDriver(_configuration);
            services.AddDefaultAuthentication(_configuration);
            services.AddSingleton<ITimelineGraphStore, TimelineGraphStore>();

            services.AddIntegrationEventHandler<UserCreatedIntegrationEventHandler, UserCreatedIntegrationEvent>();
            services.AddIntegrationEventHandler<UserDisplayNameUpdatedIntegrationEventHandler, UserDisplayNameUpdatedIntegrationEvent>();
            services.AddIntegrationEventHandler<UserProfileDescriptionUpdatedIntegrationEventHandler, UserProfileDescriptionUpdatedIntegrationEvent>();
            services.AddIntegrationEventHandler<UserProfilePictureUrlUpdatedIntegrationEventHandler, UserProfilePictureUrlUpdatedIntegrationEvent>();

            services.AddIntegrationEventHandler<KweetCreatedIntegrationEventHandler, KweetCreatedIntegrationEvent>();
            services.AddIntegrationEventHandler<KweetLikedIntegrationEventHandler, KweetLikedIntegrationEvent>();
            services.AddIntegrationEventHandler<KweetUnlikedIntegrationEventHandler, KweetUnlikedIntegrationEvent>();

            services.AddIntegrationEventHandler<UserUnfollowedIntegrationEventHandler, UserUnfollowedIntegrationEvent>();
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
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                string version = _configuration["Service:Version"];
                string title = _configuration["Service:Title"];
                app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{title} {version}"));
            }

            // Declare used exchanges!
            rabbitConfiguration.DeclareExchange("UserExchange", ExchangeType.Topic);
            rabbitConfiguration.DeclareExchange("FollowExchange", ExchangeType.Topic);
            rabbitConfiguration.DeclareExchange("KweetExchange", ExchangeType.Topic);

            rabbitConfiguration.DeclareAndBindQueueToExchange("UserExchange", "TimelineService.UserCreatedIntegrationEvent", "#.UserCreatedIntegrationEvent");
            rabbitConfiguration.DeclareAndBindQueueToExchange("UserExchange", "TimelineService.UserDisplayNameUpdatedIntegrationEvent", "#.UserDisplayNameUpdatedIntegrationEvent");
            rabbitConfiguration.DeclareAndBindQueueToExchange("UserExchange", "TimelineService.UserProfileDescriptionUpdatedIntegrationEvent", "#.UserProfileDescriptionUpdatedIntegrationEvent");
            rabbitConfiguration.DeclareAndBindQueueToExchange("UserExchange", "TimelineService.UserProfilePictureUrlUpdatedIntegrationEvent", "#.UserProfilePictureUrlUpdatedIntegrationEvent");

            rabbitConfiguration.DeclareAndBindQueueToExchange("FollowExchange", "TimelineService.UserFollowedIntegrationEvent", "#.UserFollowedIntegrationEvent");
            rabbitConfiguration.DeclareAndBindQueueToExchange("FollowExchange", "TimelineService.UserUnfollowedIntegrationEvent", "#.UserUnfollowedIntegrationEvent");

            rabbitConfiguration.DeclareAndBindQueueToExchange("KweetExchange", "TimelineService.KweetCreatedIntegrationEvent", "#.KweetCreatedIntegrationEvent");
            rabbitConfiguration.DeclareAndBindQueueToExchange("KweetExchange", "TimelineService.KweetLikedIntegrationEvent", "#.KweetLikedIntegrationEvent");
            rabbitConfiguration.DeclareAndBindQueueToExchange("KweetExchange", "TimelineService.KweetUnlikedIntegrationEvent", "#.KweetUnlikedIntegrationEvent");

            // Subscribe to integration events.
            eventBus.Subscribe<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>("TimelineService.UserCreatedIntegrationEvent");
            eventBus.Subscribe<UserDisplayNameUpdatedIntegrationEvent, UserDisplayNameUpdatedIntegrationEventHandler>("TimelineService.UserDisplayNameUpdatedIntegrationEvent");
            eventBus.Subscribe<UserProfilePictureUrlUpdatedIntegrationEvent, UserProfilePictureUrlUpdatedIntegrationEventHandler>("TimelineService.UserProfilePictureUrlUpdatedIntegrationEvent");
            eventBus.Subscribe<UserProfileDescriptionUpdatedIntegrationEvent, UserProfileDescriptionUpdatedIntegrationEventHandler>("TimelineService.UserProfileDescriptionUpdatedIntegrationEvent");

            eventBus.Subscribe<KweetCreatedIntegrationEvent, KweetCreatedIntegrationEventHandler>("TimelineService.KweetCreatedIntegrationEvent");
            eventBus.Subscribe<KweetLikedIntegrationEvent, KweetLikedIntegrationEventHandler>("TimelineService.KweetLikedIntegrationEvent");
            eventBus.Subscribe<KweetUnlikedIntegrationEvent, KweetUnlikedIntegrationEventHandler>("TimelineService.KweetUnlikedIntegrationEvent");

            eventBus.Subscribe<UserFollowedIntegrationEvent, UserFollowedIntegrationEventHandler>("TimelineService.UserFollowedIntegrationEvent");
            eventBus.Subscribe<UserUnfollowedIntegrationEvent, UserUnfollowedIntegrationEventHandler>("TimelineService.UserUnfollowedIntegrationEvent");

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
