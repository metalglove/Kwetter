using System;
using System.IO;
using System.Reflection;
using Kwetter.Services.Common.API;
using Kwetter.Services.Common.EventBus;
using Kwetter.Services.Common.EventBus.Abstractions;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.Common.Infrastructure.Integration;
using Kwetter.Services.Common.Infrastructure.MessageSerializers;
using Kwetter.Services.FollowService.API;
using Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;
using Kwetter.Services.FollowService.Infrastructure;
using Kwetter.Services.FollowService.Infrastructure.Repositories;
using Kwetter.Services.FollowService.Tests.Mocks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kwetter.Services.FollowService.Tests
{
    public abstract class TestBase
    {
        protected static ServiceProvider InitializeServices()
        {
            // required to initialise native SQLite libraries on some platforms.
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
                integrationEventMessagingConfiguration.ServiceName = "Kwetter.Services.FollowService.API";
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
            serviceCollection.AddDefaultApplicationServices(Assembly.GetAssembly(typeof(Startup)), Assembly.GetAssembly(typeof(CreateFollowCommand)));
            
            // Mock infrastructure
            serviceCollection.AddTransient<IMessageSerializer, JsonMessageSerializer>();
            serviceCollection.AddSingleton<IEventBus, EventBusMock>();
            
            serviceCollection.AddSingleton<IFactory<FollowDbContext>>(serviceProvider =>
            {
                IOptions<DbConfiguration> options = serviceProvider.GetRequiredService<IOptions<DbConfiguration>>();
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                IMediator mediator = serviceProvider.GetRequiredService<IMediator>();
                return new FollowDatabaseFactory(options, loggerFactory, mediator);
            });
            serviceCollection.AddSingleton<FollowDbContext>(p =>
            {
                FollowDbContext followDbContext = p.GetRequiredService<IFactory<FollowDbContext>>().Create();
                followDbContext.Database.EnsureCreated();
                return followDbContext;
            });
            serviceCollection.AddTransient<IAggregateUnitOfWork>(p => p.GetRequiredService<FollowDbContext>());
            serviceCollection.AddTransient<IFollowRepository, FollowRepository>();
            serviceCollection.AddIntegrationServices<FollowDbContext>(Assembly.GetAssembly(typeof(Startup)));
            return serviceCollection.BuildServiceProvider();
        }

        protected void Cleanup(ServiceProvider serviceProvider)
        {
            IOptions<DbConfiguration> options = serviceProvider.GetRequiredService<IOptions<DbConfiguration>>();
            File.Delete(options.Value.ConnectionString.Replace("Data Source=",""));
        }
    }
}