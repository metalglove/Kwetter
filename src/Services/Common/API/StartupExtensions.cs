using EventStore.Client;
using FluentValidation;
using Kwetter.Services.Common.API.Behaviours;
using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.Common.Application.Eventing.Store;
using Kwetter.Services.Common.Infrastructure.Authorization;
using Kwetter.Services.Common.Infrastructure.Behaviours;
using Kwetter.Services.Common.Infrastructure.Eventing;
using Kwetter.Services.Common.Infrastructure.Eventing.Bus;
using Kwetter.Services.Common.Infrastructure.EventSerializers;
using Kwetter.Services.Common.Infrastructure.Integration;
using Kwetter.Services.Common.Infrastructure.RabbitMq;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
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

                swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer ...')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
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
            serviceCollection.Configure<AuthorizationConfiguration>(configuration.GetSection("Authorization").Bind);
            serviceCollection.Configure<MessagingConfiguration>(configuration.GetSection("Messaging").Bind);
            serviceCollection.Configure<EventStoreConfiguration>(configuration.GetSection("EventStore").Bind);
            serviceCollection.Configure<ServiceConfiguration>(configuration.GetSection("Service").Bind);
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
            serviceCollection.AddMediatR(applicationAssembly, Assembly.GetAssembly(typeof(AnyDomainEventHandler<>)));
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
            serviceCollection.AddTransient<IEventSerializer, JsonEventSerializer>();
            serviceCollection.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            serviceCollection.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
            serviceCollection.AddSingleton<IEventBus, EventBus>();
            serviceCollection.AddSingleton<EventStoreClient>((serviceProvider) =>
            {
                EventStoreConfiguration eventStoreConfiguration = serviceProvider.GetRequiredService<IOptions<EventStoreConfiguration>>().Value;
                EventStoreClientSettings settings = EventStoreClientSettings.Create(eventStoreConfiguration.ConnectionUrl);
                return new EventStoreClient(settings);
            });
            // TODO: Fix event store to be scoped again!!
            serviceCollection.AddSingleton<IEventStore, Infrastructure.Eventing.Store.EventStore>();
            serviceCollection.AddSingleton<IIntegrationEventService, IntegrationEventService>();
            return serviceCollection;
        }

        /// <summary>
        /// Adds the defaults authentication implementation.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration containing the Authorization configuration.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddDefaultAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IConfigurationRetriever<JsonWebKeySet>, JsonWebKeySetRetriever>();
            serviceCollection.AddSingleton<IConfigurationManager<JsonWebKeySet>>((a) =>
            {
                IConfigurationRetriever<JsonWebKeySet> retriever = a.GetRequiredService<IConfigurationRetriever<JsonWebKeySet>>();
                return new ConfigurationManager<JsonWebKeySet>($"{configuration["Authorization:JwksUri"]}", retriever); ;
            });

            serviceCollection
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtBearerOptions, JwtTokenAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, o => { });

            return serviceCollection;
        }
    }
}
