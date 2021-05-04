using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.Common.Infrastructure.Behaviours;
using Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand;
using Kwetter.Services.FollowService.API.Application.IntegrationEventHandlers.UserCreated;
using Kwetter.Services.FollowService.API.Application.IntegrationEventHandlers.UserDisplayNameUpdated;
using Kwetter.Services.FollowService.API.Application.IntegrationEventHandlers.UserProfilePictureUrlUpdated;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.FollowService.Infrastructure;
using Kwetter.Services.FollowService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Kwetter.Services.FollowService.API
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
            services.AddDefaultApplicationServices(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(CreateFollowCommand)));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddDefaultInfrastructureServices();
            services.AddEventStore();
            services.AddDefaultAuthentication(_configuration);
            services.AddScoped<IFactory<FollowDbContext>, FollowDatabaseFactory>();
            services.AddScoped<FollowDbContext>(p => p.GetRequiredService<IFactory<FollowDbContext>>().Create());
            services.AddScoped<IAggregateUnitOfWork>(p => p.GetRequiredService<IFactory<FollowDbContext>>().Create());
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddControllers();
            services.AddSwagger(_configuration);
            services.VerifyDatabaseConnection<FollowDbContext>();
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

            // Subscribe to integration events.
            eventBus.Subscribe<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>(
                exchangeName: "UserExchange",
                queueName: $"UserService.Integration.UserCreatedIntegrationEvent",
                eventHandler: new UserCreatedIntegrationEventHandler(serviceScopeFactory));

            eventBus.Subscribe<UserDisplayNameUpdatedIntegrationEvent, UserDisplayNameUpdatedIntegrationEventHandler>(
                exchangeName: "UserExchange",
                queueName: $"UserService.Integration.UserDisplayNameUpdatedIntegrationEvent",
                eventHandler: new UserDisplayNameUpdatedIntegrationEventHandler(serviceScopeFactory));

            eventBus.Subscribe<UserProfilePictureUrlUpdatedIntegrationEvent, UserProfilePictureUrlUpdatedIntegrationEventHandler>(
                exchangeName: "UserExchange",
                queueName: $"UserService.Integration.UserProfilePictureUrlUpdatedIntegrationEvent",
                eventHandler: new UserProfilePictureUrlUpdatedIntegrationEventHandler(serviceScopeFactory));

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
