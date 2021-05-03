using Kwetter.Services.Common.Tests;
using Kwetter.Services.UserService.API;
using Kwetter.Services.UserService.API.Application.Commands.CreateUserCommand;
using Kwetter.Services.UserService.API.Application.IntegrationEventHandlers.IdentityCreatedIntegration;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.UserService.Infrastructure;
using Kwetter.Services.UserService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.Tests.IntegrationEventHandlers
{
    [TestClass]
    public class IdentityCreatedIntegrationEventHandlerTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IdentityCreatedIntegrationEventHandler IdentityCreatedIntegrationEventHandler { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider = InitializeServices<UserDbContext, UserDatabaseFactory, UserRepository, UserAggregate>(typeof(Startup), typeof(CreateUserCommand), "UserService");
            IdentityCreatedIntegrationEventHandler = new IdentityCreatedIntegrationEventHandler(ServiceProvider.GetRequiredService<IServiceScopeFactory>());
        }

        [TestCleanup]

        public void Cleanup()
        {
            Cleanup(ServiceProvider);
        }

        [TestMethod]
        public async Task Should_Create_User_Through_IdentityCreatedIntegrationEventHandler()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "Glovali";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new(userId, userDisplayName, userProfilePictureUrl);

            // Act
            await IdentityCreatedIntegrationEventHandler.HandleAsync(identityCreatedIntegrationEvent, CancellationToken.None);

            // Assert
            IUserRepository userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            UserAggregate user = await userRepository.FindByIdAsync(userId, CancellationToken.None);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_IdentityCreatedIntegrationEventHandler_Due_To_DisplayName_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new(userId, userDisplayName, userProfilePictureUrl);

            // Act
            await IdentityCreatedIntegrationEventHandler.HandleAsync(identityCreatedIntegrationEvent, CancellationToken.None);

            // Assert
            IUserRepository userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            UserAggregate user = await userRepository.FindByIdAsync(userId, CancellationToken.None);
            Assert.IsNull(user);
        }
    }
}
