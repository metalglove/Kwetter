﻿using FluentValidation;
using Kwetter.Services.Common.API.Behaviours;
using Kwetter.Services.Common.EventBus;
using Kwetter.Services.Common.EventBus.Abstractions;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.Common.Infrastructure.Behaviours;
using Kwetter.Services.Common.Infrastructure.Integration;
using Kwetter.Services.Common.Infrastructure.MessageSerializers;
using Kwetter.Services.Common.Infrastructure.RabbitMq;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Reflection;

namespace Kwetter.Services.Common.API
{
    /// <summary>
    /// Represents the <see cref="StartupExtensions"/> class.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Adds swagger to the service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            string version = configuration["Service:Version"];
            string title = configuration["Service:Title"];

            serviceCollection.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc(name: version, new OpenApiInfo()
                {
                    Title = title,
                    Version = version
                });
            });
            return serviceCollection;
        }

        /// <summary>
        /// Adds and binds configurations into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();
            serviceCollection.Configure<DbConfiguration>(configuration.GetSection("Database").Bind);
            //serviceCollection.Configure<AuthorizationConfiguration>(configuration.GetSection("Authorization").Bind);
            serviceCollection.Configure<DbConfiguration>("IntegrationDatabase", configuration.GetSection("Integration:Database").Bind);
            serviceCollection.Configure<IntegrationEventMessagingConfiguration>(configuration.GetSection("Integration").Bind);
            serviceCollection.Configure<MessagingConfiguration>(configuration.GetSection("Messaging").Bind);
            return serviceCollection;
        }

        /// <summary>
        /// Adds the AutoMapper mappings from the provided assembly.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="mappingAssembly">The mapping assembly.</param>
        /// <param name="applicationAssembly">The application assembly.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddDefaultApplicationServices(this IServiceCollection serviceCollection, Assembly mappingAssembly, Assembly applicationAssembly)
        {
            serviceCollection.AddAutoMapper(mappingAssembly, Assembly.GetAssembly(typeof(StartupExtensions)));
            serviceCollection.AddValidatorsFromAssembly(applicationAssembly);
            serviceCollection.AddMediatR(applicationAssembly);
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            return serviceCollection;
        }

        /// <summary>
        /// Adds the default infrastructure services.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddDefaultInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMessageSerializer, JsonMessageSerializer>();
            serviceCollection.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            serviceCollection.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
            serviceCollection.AddSingleton<IEventBus, Infrastructure.EventBus>();
            return serviceCollection;
        }

        /// <summary>
        /// Adds the integration services into the service collection.
        /// </summary>
        /// <typeparam name="TContext">The service context.</typeparam>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="integrationEventAssembly">The integration event assembly.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddIntegrationServices<TContext>(this IServiceCollection serviceCollection, Assembly integrationEventAssembly) where TContext : UnitOfWork<TContext>
        {
            serviceCollection.AddSingleton<IFactory<IntegrationEventLogDbContext>>(serviceProvider =>
            {
                IOptionsMonitor<DbConfiguration> options = serviceProvider.GetRequiredService<IOptionsMonitor<DbConfiguration>>();
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return new IntegrationEventLogDbFactory(options.Get("IntegrationDatabase"), loggerFactory);
            });

            serviceCollection.AddTransient<IntegrationEventLogDbContext>(p => p.GetRequiredService<IFactory<IntegrationEventLogDbContext>>().Create());
            serviceCollection.VerifyDatabaseConnection<IntegrationEventLogDbContext>();
            serviceCollection.AddSingleton<IIntegrationEventService, IntegrationEventService<TContext>>();
            serviceCollection.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>(serviceProvider =>
            {
                IMessageSerializer messageSerializer = serviceProvider.GetRequiredService<IMessageSerializer>();
                IntegrationEventLogDbContext integrationEventLogDbContext = serviceProvider.GetRequiredService<IntegrationEventLogDbContext>();
                return new IntegrationEventLogService(messageSerializer, integrationEventLogDbContext, integrationEventAssembly);
            });
            return serviceCollection;
        }
    }
}
