using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.Common.Infrastructure.Behaviours;
using Kwetter.Services.Common.Infrastructure.RabbitMq;
using Kwetter.Services.UserService.API.Application.IntegrationEventHandlers.IdentityCreated;
using Kwetter.Services.UserService.API.Controllers;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.UserService.Infrastructure;
using Kwetter.Services.UserService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Kwetter.Services.UserService.API
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
            services.AddDefaultApplicationServices(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(UserController)));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddDefaultInfrastructureServices();
            services.AddEventStore();
            services.AddDefaultAuthentication(_configuration);
            services.AddScoped<IFactory<UserDbContext>, UserDatabaseFactory>();
            services.AddScoped<UserDbContext>(p => p.GetRequiredService<IFactory<UserDbContext>>().Create());
            services.AddScoped<IAggregateUnitOfWork>(p => p.GetRequiredService<IFactory<UserDbContext>>().Create());
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddIntegrationEventHandler<IdentityCreatedIntegrationEventHandler, IdentityCreatedIntegrationEvent>();

            services.AddControllers();
            services.AddSwagger(_configuration);
            services.VerifyDatabaseConnection<UserDbContext>();
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
            rabbitConfiguration.DeclareExchange("AuthorizationExchange", ExchangeType.Direct);
            rabbitConfiguration.DeclareExchange("UserExchange", ExchangeType.Topic);
            rabbitConfiguration.DeclareAndBindQueueToExchange("AuthorizationExchange", "UserService.IdentityCreatedIntegrationEvent", "IdentityCreatedIntegrationEvent");

            // Subscribe to integration events.
            eventBus.Subscribe<IdentityCreatedIntegrationEvent, IdentityCreatedIntegrationEventHandler>("UserService.IdentityCreatedIntegrationEvent");

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
