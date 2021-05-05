using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Application.Dtos;
using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.Common.Application.Eventing.Bus;
using Kwetter.Services.Common.Application.Eventing.Integration;
using Kwetter.Services.Common.Application.Eventing.Store;
using Kwetter.Services.Common.Application.Interfaces;
using Kwetter.Services.Common.Domain;
using Kwetter.Services.Common.Domain.Persistence;
using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.Common.Infrastructure.Authorization;
using Kwetter.Services.Common.Infrastructure.Behaviours;
using Kwetter.Services.Common.Infrastructure.Eventing;
using Kwetter.Services.Common.Infrastructure.EventSerializers;
using Kwetter.Services.Common.Infrastructure.Integration;
using Kwetter.Services.Common.Tests.Mocks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Principal;

namespace Kwetter.Services.Common.Tests
{
    /// <summary>
    /// Represents the <see cref="TestBase"/> class.
    /// Holds functionality for proper integration tests.
    /// </summary>
    public abstract class TestBase
    {
        public Dictionary<string, ClaimDto> Claims { get; }
        public ClaimsDto ClaimsDto { get; }
        public Guid AuthorizedUserId { get; }
        public string AuthorizedUserName { get; }

        public TestBase()
        {
            AuthorizedUserId = Guid.NewGuid();
            AuthorizedUserName = "kwetterman";
            Claims = new()
            {
                { "UserId", new ClaimDto() { Name = "UserId", Value = AuthorizedUserId.ToString() } },
                { "UserName", new ClaimDto() { Name = "UserName", Value = AuthorizedUserName } },
                { "User", new ClaimDto() { Name = "User", Value = "true" } },
                { "name", new ClaimDto() { Name = "name", Value = "kwetter user" } },
                { "picture", new ClaimDto() { Name = "picture", Value = "https://lh3.googleusercontent.com/a/AATXAJx0qUdfbGJlciCWpfjoZWJDWyIv9o2VAEr0rkpa=s96-c" } },
                { "email", new ClaimDto() { Name = "email", Value = "kwetteruser@gmail.com" } }
            };
            ClaimsDto = new()
            {
                Audience = "s64-1-vetis",
                ExpirationTimeSeconds = 1620256552,
                IssuedAtTimeSeconds = 1620252952,
                Issuer = "https://securetoken.google.com/s64-1-vetis",
                Subject = "18ONCeiJZxZ5D4rmJgAXEkiRVor2",
                Claims = Claims
            };
        }
        /// <summary>
        /// Initializes the services for the tests.
        /// </summary>
        /// <param name="startUpType">The start up class type.</param>
        /// <param name="applicationType">The application type.</param>
        /// <param name="serviceName">The service name.</param>
        /// <typeparam name="TDbContext">The database context/</typeparam>
        /// <typeparam name="TDatabaseFactory">The database factory type.</typeparam>
        /// <typeparam name="TRepository">The repository type.</typeparam>
        /// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
        /// <returns>Returns the service collection.</returns>
        protected IServiceCollection InitializeServices<TDbContext, TDatabaseFactory, TRepository, TAggregateRoot>(
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

            serviceCollection.AddSingleton<ITokenVerifier>((a) =>
            {
                return new TokenVerifierMock(ClaimsDto);
            });
            serviceCollection
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtBearerOptions, JwtTokenAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, o => { });

            return serviceCollection;
        }

        /// <summary>
        /// Initializes the service provier for the tests.
        /// </summary>
        /// <param name="startUpType">The start up class type.</param>
        /// <param name="applicationType">The application type.</param>
        /// <param name="serviceName">The service name.</param>
        /// <typeparam name="TDbContext">The database context/</typeparam>
        /// <typeparam name="TDatabaseFactory">The database factory type.</typeparam>
        /// <typeparam name="TRepository">The repository type.</typeparam>
        /// <typeparam name="TAggregateRoot">The aggregate root type.</typeparam>
        /// <returns>Returns the service provider.</returns>
        protected ServiceProvider InitializeServiceProvider<TDbContext, TDatabaseFactory, TRepository, TAggregateRoot>(
            Type startUpType, Type applicationType, string serviceName) 
            where TDbContext : UnitOfWork<TDbContext>, IAggregateUnitOfWork
            where TDatabaseFactory : DatabaseFactory<TDbContext>, new()
            where TAggregateRoot : Entity, IAggregateRoot
            where TRepository : class, IRepository<TAggregateRoot>
        {
            ServiceProvider serviceProvider = InitializeServices<TDbContext, TDatabaseFactory, TRepository, TAggregateRoot>(startUpType, applicationType, serviceName).BuildServiceProvider();
            return serviceProvider;
        }

        /// <summary>
        /// Creates an authorized controller instance.
        /// </summary>
        /// <typeparam name="TController">The controller type.</typeparam>
        /// <param name="mediator">The mediator.</param>
        /// <returns>Returns the authorized controller instance.</returns>
        protected TController CreateAuthorizedController<TController>(IMediator mediator) where TController : ControllerBase
        {
            ClaimsIdentity id = new(new List<Claim>() 
            { 
                new Claim("UserId", AuthorizedUserId.ToString()),
                new Claim("UserName", AuthorizedUserName),
                new Claim("User", "true"),
            });
            Type controllerType = typeof(TController);
            ConstructorInfo constructor = controllerType.GetConstructor(new Type[] { typeof(IMediator) });
            object emptyInstance = FormatterServices.GetUninitializedObject(controllerType);
            constructor.Invoke(emptyInstance, new object[] { mediator });
            TController controller = emptyInstance as TController;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new GenericPrincipal(id, null)
                }
            };
            return controller;
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