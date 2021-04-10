using System;
using System.IO;
using System.Reflection;
using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Domain;
using Kwetter.Services.Common.Domain.Persistence;
using Kwetter.Services.Common.EventBus;
using Kwetter.Services.Common.EventBus.Abstractions;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.Common.Infrastructure.Configurations;
using Kwetter.Services.Common.Infrastructure.Integration;
using Kwetter.Services.Common.Infrastructure.MessageSerializers;
using Kwetter.Services.Common.Tests.Mocks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        /// <param name="databaseFactory">The database factory.</param>
        /// <typeparam name="TDbContext">The database context/</typeparam>
        /// <typeparam name="TDatabaseFactory">The database factory type.</typeparam>
        /// <typeparam name="TRepository">The repository type.</typeparam>
        /// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
        /// <returns>Returns a service provider.</returns>
        protected static ServiceProvider InitializeServices<TDbContext, TDatabaseFactory, TRepository, TAggregateRoot>(
            Type startUpType, Type applicationType, string serviceName, Func<IOptions<DbConfiguration>, ILoggerFactory, IMediator, TDatabaseFactory> databaseFactory) 
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
            serviceCollection.Configure<DbConfiguration>("IntegrationDatabase", dbConfig =>
            {
                dbConfig.UseLazyLoading = true;
                dbConfig.ConnectionString = "mssql-in-memory";
                dbConfig.DbProvider = "mssql-in-memory";
            });
            serviceCollection.Configure<IntegrationEventMessagingConfiguration>(integrationEventMessagingConfiguration =>
            {
                integrationEventMessagingConfiguration.ServiceName = $"Kwetter.Services.{serviceName}.API";
                integrationEventMessagingConfiguration.MessageQueueName = "IntegrationEventLog";
            });
            
            serviceCollection.Configure<MessagingConfiguration>(messagingConfiguration =>
            {
                messagingConfiguration.Host = "";
                messagingConfiguration.Password = "";
                messagingConfiguration.Port = 8493;
                messagingConfiguration.VirtualHost = "/";
            });
            
            serviceCollection.AddLogging(p => p.AddConsole());
            serviceCollection.AddDefaultApplicationServices(Assembly.GetAssembly(startUpType), Assembly.GetAssembly(applicationType));
            
            // Mock infrastructure
            serviceCollection.AddTransient<IMessageSerializer, JsonMessageSerializer>();
            serviceCollection.AddSingleton<IEventBus, EventBusMock>();
            
            serviceCollection.AddSingleton<IFactory<TDbContext>>(serviceProvider =>
            {
                IOptions<DbConfiguration> options = serviceProvider.GetRequiredService<IOptions<DbConfiguration>>();
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                IMediator mediator = serviceProvider.GetRequiredService<IMediator>();
                return databaseFactory(options, loggerFactory, mediator);
            });
            serviceCollection.AddSingleton<TDbContext>(p =>
            {
                TDbContext followDbContext = p.GetRequiredService<IFactory<TDbContext>>().Create();
                followDbContext.Database.EnsureCreated();
                return followDbContext;
            });
            serviceCollection.AddTransient<IAggregateUnitOfWork>(p => p.GetRequiredService<TDbContext>());
            Type repositoryImplementationType = typeof(TRepository);
            serviceCollection.AddTransient(repositoryImplementationType.GetInterfaces()[0], repositoryImplementationType);
            serviceCollection.AddIntegrationServices<TDbContext>(Assembly.GetAssembly(startUpType));
            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Cleans up the created sqlite database file
        /// </summary>
        /// <param name="serviceProvider">The serviceName provider.</param>
        protected void Cleanup(ServiceProvider serviceProvider)
        {
            IOptions<DbConfiguration> options = serviceProvider.GetRequiredService<IOptions<DbConfiguration>>();
            File.Delete(options.Value.ConnectionString.Replace("Data Source=",""));
        }
    }
}