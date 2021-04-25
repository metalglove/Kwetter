using Kwetter.Services.Common.API;
using Kwetter.Services.TimelineService.API.Application.Queries.KweetTimelineQuery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;
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
            services.AddDefaultAuthentication(_configuration);

            RedisConfiguration conf = new()
            {
                AbortOnConnectFail = true,
                Hosts = new[] { new RedisHost { Host = "localhost", Port = 6379 } },
                AllowAdmin = true,
                ConnectTimeout = 5000,
                Database = 0,
                PoolSize = 50,
                ServerEnumerationStrategy = new ServerEnumerationStrategy()
                {
                    Mode = ServerEnumerationStrategy.ModeOptions.All,
                    TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                    UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                }
            };

            services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(conf);

            //services.AddScoped<IFactory<KweetDbContext>, KweetDatabaseFactory>();
            //services.AddScoped<KweetDbContext>(p => p.GetRequiredService<IFactory<KweetDbContext>>().Create());
            //services.AddScoped<IAggregateUnitOfWork>(p => p.GetRequiredService<IFactory<KweetDbContext>>().Create());
            //services.AddScoped<IKweetRepository, KweetRepository>();
            services.AddControllers();
            services.AddSwagger(_configuration);
            //services.VerifyDatabaseConnection<KweetDbContext>();
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
