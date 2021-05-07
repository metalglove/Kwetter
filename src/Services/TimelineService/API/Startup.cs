using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserCreated;
using Kwetter.Services.TimelineService.API.Application.Queries.KweetTimelineQuery;
using Kwetter.Services.TimelineService.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddControllers();
            services.AddSwagger(_configuration);
        }

        /// <summary>
        /// Configures the application request response flow (middleware).
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web host environment.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="serviceScopeFactory">The service scope factory.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEventBus eventBus, IServiceScopeFactory serviceScopeFactory)
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
            eventBus.DeclareExchange("UserExchange", Common.Application.Eventing.ExchangeType.FANOUT);
            eventBus.DeclareExchange("FollowExchange", Common.Application.Eventing.ExchangeType.FANOUT);
            eventBus.DeclareExchange("KweetExchange", Common.Application.Eventing.ExchangeType.FANOUT);

            // Subscribe to integration events.
            eventBus.Subscribe<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>(
                exchangeName: "UserExchange",
                queueName: $"UserService.Integration.UserCreatedIntegrationEvent",
                eventHandler: new UserCreatedIntegrationEventHandler(serviceScopeFactory));

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