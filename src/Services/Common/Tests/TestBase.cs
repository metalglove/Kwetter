using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.Common.Application.Eventing.Store;
using Kwetter.Services.Common.Domain;
using Kwetter.Services.Common.Domain.Persistence;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.Common.Infrastructure.Behaviours;
using Kwetter.Services.Common.Infrastructure.Eventing;
using Kwetter.Services.Common.Infrastructure.EventSerializers;
using Kwetter.Services.Common.Infrastructure.Integration;
using Kwetter.Services.Common.Tests.Mocks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Reflection;

namespace Kwetter.Services.Common.Tests
{
    /// <summary>
    /// Represents the <see cref="TestBase"/> class.
    /// Holds functionality for proper integration tests.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        /// Initializes the services for the test.
        /// </summary>
        /// <param name="startUpType">The start up class type.</param>
        /// <param name="applicationType">The application type.</param>
        /// <param name="serviceName">The service name.</param>
        /// <typeparam name="TDbContext">The database context/</typeparam>
        /// <typeparam name="TDatabaseFactory">The database factory type.</typeparam>
        /// <typeparam name="TRepository">The repository type.</typeparam>
        /// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
        /// <returns>Returns a service provider.</returns>
        protected static ServiceProvider InitializeServices<TDbContext, TDatabaseFactory, TRepository, TAggregateRoot>(
            Type startUpType, Type applicationType, string serviceName) 
            where TDbContext : UnitOfWork<TDbContext>, IAggregateUnitOfWork
            where TDatabaseFactory : DatabaseFactory<TDbContext>, new()
            where TAggregateRoot : Entity, IAggregateRoot
            where TRepository : class, IRepository<TAggregateRoot>
        {
            // Required to initialise native SQLite libraries on some platforms.
            SQLitePCL.Batteries_V2.Init();

            // https://github.com/aspnet/EntityFrameworkCore/issues/9994#issuecomment-508588678
            SQLitePCL.raw.sqlite3_config(3);
            
            // Prepare services.
            IServiceCollection serviceCollection = new ServiceCollection();

            // Manually add database configuration.
            // Will use InMemory for empty connection string and provider.
            serviceCollection.AddOptions();
            serviceCollection.Configure<DbConfiguration>(dbConfig =>
            {
                dbConfig.UseLazyLoading = true;
                dbConfig.ConnectionString = $"Data Source={Directory.GetCurrentDirectory()}/{Guid.NewGuid()}.db";
                dbConfig.DbProvider = "sqlite";
            });
            serviceCollection.Configure<MessagingConfiguration>(messagingConfiguration =>
            {
                messagingConfiguration.Host = "";
                messagingConfiguration.Password = "";
                messagingConfiguration.Port = 8493;
                messagingConfiguration.VirtualHost = "/";
            });

            serviceCollection.Configure<ServiceConfiguration>(serviceConfiguration =>
            {
                serviceConfiguration.Title = $"Kwetter.Services.{serviceName}.API";
                serviceConfiguration.ShortTitle = serviceName;
                serviceConfiguration.Version = "v1";
            });

            serviceCollection.AddLogging(p => p.AddConsole());
            serviceCollection.AddDefaultApplicationServices(Assembly.GetAssembly(startUpType), Assembly.GetAssembly(applicationType));
            serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

            // Mock infrastructure
            serviceCollection.AddTransient<IEventSerializer, JsonEventSerializer>();
            serviceCollection.AddSingleton<IEventBus, EventBusMock>();
            serviceCollection.AddSingleton<IEventStore, EventStoreMock>();
            serviceCollection.AddScoped(typeof(INotificationHandler<>), typeof(AnyDomainEventHandler<>));
            serviceCollection.AddScoped<IIntegrationEventService, IntegrationEventService>();
            serviceCollection.AddScoped<IFactory<TDbContext>, TDatabaseFactory>();
            serviceCollection.AddScoped<TDbContext>(p =>
            {
                TDbContext followDbContext = p.GetRequiredService<IFactory<TDbContext>>().Create();
                followDbContext.Database.EnsureCreated();
                return followDbContext;
            });
            serviceCollection.AddTransient<IAggregateUnitOfWork>(p => p.GetRequiredService<TDbContext>());
            Type repositoryImplementationType = typeof(TRepository);
            serviceCollection.AddTransient(repositoryImplementationType.GetInterfaces()[0], repositoryImplementationType);
            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Cleans up the created sqlite database file
        /// </summary>
        /// <param name="serviceProvider">The serviceName provider.</param>
        protected static void Cleanup(ServiceProvider serviceProvider)
        {
            IOptions<DbConfiguration> options = serviceProvider.GetRequiredService<IOptions<DbConfiguration>>();
            string path = options.Value.ConnectionString.Replace("Data Source=", "");
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                serviceProvider.GetRequiredService<ILogger<TestBase>>().LogError($"Failed to delete the temporary database file: {path}");
            }
        }
    }
}