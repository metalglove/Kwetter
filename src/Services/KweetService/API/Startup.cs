using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;
using Kwetter.Services.KweetService.Infrastructure;
using Kwetter.Services.KweetService.Infrastructure.Repositories;
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

namespace Kwetter.Services.KweetService.API
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
            services.AddDefaultApplicationServices(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(CreateKweetCommand)));
            services.AddDefaultInfrastructureServices();
            services.AddDefaultAuthentication(_configuration);
            services.AddSingleton<IFactory<KweetDbContext>>(serviceProvider =>
            {
                IOptions<DbConfiguration> options = serviceProvider.GetRequiredService<IOptions<DbConfiguration>>();
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                IMediator mediator = serviceProvider.GetRequiredService<IMediator>();
                return new KweetDatabaseFactory(options, loggerFactory, mediator);
            });
            services.AddTransient<KweetDbContext>(p => p.GetRequiredService<IFactory<KweetDbContext>>().Create());
            services.AddTransient<IAggregateUnitOfWork>(p => p.GetRequiredService<IFactory<KweetDbContext>>().Create());
            services.AddTransient<IKweetRepository, KweetRepository>();
            services.AddControllers();
            services.AddSwagger(_configuration);
            services.VerifyDatabaseConnection<KweetDbContext>();
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