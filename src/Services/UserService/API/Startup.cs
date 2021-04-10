using System.Diagnostics.CodeAnalysis;
using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.UserService.API.Application.Commands.CreateUserCommand;
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
using Microsoft.Extensions.Options;
using System.Reflection;
using Kwetter.Services.Common.Infrastructure.Configurations;

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
            services.AddDefaultApplicationServices(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(CreateUserCommand)));
            services.AddDefaultInfrastructureServices();
            services.AddDefaultAuthentication(_configuration);
            services.AddSingleton<IFactory<UserDbContext>>(serviceProvider =>
            {
                IOptions<DbConfiguration> options = serviceProvider.GetRequiredService<IOptions<DbConfiguration>>();
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                IMediator mediator = serviceProvider.GetRequiredService<IMediator>();
                return new UserDatabaseFactory(options, loggerFactory, mediator);
            });
            services.AddTransient<UserDbContext>(p => p.GetRequiredService<IFactory<UserDbContext>>().Create());
            services.AddTransient<IAggregateUnitOfWork>(p => p.GetRequiredService<IFactory<UserDbContext>>().Create());
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddIntegrationServices<UserDbContext>(Assembly.GetAssembly(typeof(Startup)));
            services.AddControllers();
            services.AddSwagger(_configuration);
            services.VerifyDatabaseConnection<UserDbContext>();
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
