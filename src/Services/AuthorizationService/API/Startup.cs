using Kwetter.Services.AuthorizationService.API.Application.Commands.ClaimsCommand;
using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Infrastructure;
using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.AuthorizationService.Infrastructure.Repositories;
using Kwetter.Services.AuthorizationService.Infrastructure.Services;
using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.Common.Infrastructure.Behaviours;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Kwetter.Services.AuthorizationService.API
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
            services.AddDefaultApplicationServices(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(ClaimsCommand)));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddLogging(p => p.AddConsole());
            services.AddDefaultInfrastructureServices();
            services.AddEventStore();
            services.AddDefaultAuthentication(_configuration);
            services.AddSingleton<IAuthorizationService, GoogleAuthorizationService>();
            services.AddScoped<IFactory<IdentityDbContext>, IdentityDatabaseFactory>();
            services.AddScoped<IdentityDbContext>(p => p.GetRequiredService<IFactory<IdentityDbContext>>().Create());
            services.AddScoped<IAggregateUnitOfWork>(p => p.GetRequiredService<IFactory<IdentityDbContext>>().Create());
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddControllers();
            services.AddSwagger(_configuration);
            services.VerifyDatabaseConnection<IdentityDbContext>();
        }

        /// <summary>
        /// Configures the application request response flow (middleware).
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web host environment.</param>
        /// <param name="eventBus">The event bus</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEventBus eventBus)
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
            eventBus.DeclareExchange("AuthorizationExchange", Common.Application.Eventing.ExchangeType.DIRECT);

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
