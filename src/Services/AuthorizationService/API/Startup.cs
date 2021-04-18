using Kwetter.Services.AuthorizationService.API.Application.Queries.AuthorizationQuery;
using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Infrastructure;
using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.AuthorizationService.Infrastructure.Repositories;
using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            services.AddDefaultApplicationServices(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(AuthorizationQuery)));
            services.AddLogging(p => p.AddConsole());
            services.AddDefaultInfrastructureServices();
            services.AddDefaultAuthentication(_configuration);
            services.AddHttpClient<IAuthorizationService, Infrastructure.Services.AuthorizationService>();
            services.AddSingleton<IFactory<IdentityDbContext>>(serviceProvider =>
            {
                IOptions<DbConfiguration> options = serviceProvider.GetRequiredService<IOptions<DbConfiguration>>();
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                IMediator mediator = serviceProvider.GetRequiredService<IMediator>();
                return new IdentityDatabaseFactory(options, loggerFactory, mediator);
            });
            services.AddTransient<IdentityDbContext>(p => p.GetRequiredService<IFactory<IdentityDbContext>>().Create());
            services.AddTransient<IAggregateUnitOfWork>(p => p.GetRequiredService<IFactory<IdentityDbContext>>().Create());
            services.AddTransient<IIdentityRepository, IdentityRepository>();
            services.AddControllers();
            services.AddSwagger(_configuration);
            services.VerifyDatabaseConnection<IdentityDbContext>();
        }

        /// <summary>
        /// Configures the application request response flow (middleware).
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                string version = _configuration["Service:Version"];
                string title = _configuration["Service:Title"];
                app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{title} {version}"));
            }

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
