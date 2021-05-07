using Kwetter.Services.Common.API;
using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.UserService.API;
using Kwetter.Services.UserService.API.Application;
using Kwetter.Services.UserService.API.Application.IntegrationEventHandlers.IdentityCreated;
using Kwetter.Services.UserService.API.Controllers;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.UserService.Domain.Exceptions;
using Kwetter.Services.UserService.Infrastructure;
using Kwetter.Services.UserService.Infrastructure.Repositories;
using MediatR;
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
        public IServiceScopeFactory ServiceScopeFactory { get; set; }
        public IServiceScope Scope { get; set; }
        public IMediator Mediator { get; set; }
        public IUserRepository UserRepository { get; set; }

        [TestInitialize]
        public async Task Initialize()
        {
            IServiceCollection services = InitializeServices<UserDbContext, UserDatabaseFactory, UserRepository, UserAggregate>(typeof(Startup), typeof(UserController), "UserService");
            services.AddIntegrationEventHandler<IdentityCreatedIntegrationEventHandler, IdentityCreatedIntegrationEvent>();
            ServiceProvider = services.BuildServiceProvider();
            ServiceScopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();
            Scope = ServiceScopeFactory.CreateScope();
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new()
            {
                UserId = AuthorizedUserId,
                GivenName = "Glovali",
                UserName = AuthorizedUserName,
                ProfilePictureUrl = userProfilePictureUrl
            };
            Mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();
            UserRepository = Scope.ServiceProvider.GetRequiredService<IUserRepository>();
            CommandResponse _ = await Mediator.Send(identityCreatedIntegrationEvent, CancellationToken.None);
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
            const string userDisplayName = "Glovaliman2";
            const string userName = "supsssermario";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new() 
            { 
                UserId = userId,
                GivenName = userDisplayName, 
                UserName = userName,
                ProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            await Mediator.Send(identityCreatedIntegrationEvent, CancellationToken.None);

            // Assert
            UserAggregate user = await UserRepository.FindByIdAsync(userId, CancellationToken.None);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_IdentityCreatedIntegrationEventHandler_Due_To_DisplayName_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "";
            const string userName = "supermario";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new()
            {
                UserId = userId,
                GivenName = userDisplayName,
                UserName = userName,
                ProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            UserDomainException userDomainException = await Assert.ThrowsExceptionAsync<UserDomainException>(() => Mediator.Send(identityCreatedIntegrationEvent, CancellationToken.None));
            
            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The display name is null, empty or contains only whitespaces."));

        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_IdentityCreatedIntegrationEventHandler_Due_To_UserId_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.Empty;
            const string userDisplayName = "Hello Man 1";
            const string userName = "supeffrmario";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new()
            {
                UserId = userId,
                GivenName = userDisplayName,
                UserName = userName,
                ProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            UserDomainException userDomainException = await Assert.ThrowsExceptionAsync<UserDomainException>(() => Mediator.Send(identityCreatedIntegrationEvent, CancellationToken.None));

            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The user id is empty."));
        }


        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_IdentityCreatedIntegrationEventHandler_Due_To_DisplayName_Exceeding_64_Length()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "sdfdsfsfsdfdsfdsfdfdsfdsfdsfdsfdsfdsfdsfdfdfdfdfdfdfdfddfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdffdfdfdff";
            const string userName = "supeffrmario";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new() 
            { 
                UserId = userId,
                GivenName = userDisplayName, 
                UserName = userName,
                ProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            UserDomainException userDomainException = await Assert.ThrowsExceptionAsync<UserDomainException>(() => Mediator.Send(identityCreatedIntegrationEvent, CancellationToken.None));

            // Assert
            Assert.IsTrue(userDomainException.Message.Equals("The length of the display name exceeded 64 characters."));
        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_IdentityCreatedIntegrationEventHandler_Due_To_UserId_Already_Existing()
        {
            // Arrange
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new()
            {
                UserId = AuthorizedUserId,
                GivenName = "Glovali",
                UserName = AuthorizedUserName,
                ProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            UserIntegrationException userIntegrationException = await Assert.ThrowsExceptionAsync<UserIntegrationException>(() => Mediator.Send(identityCreatedIntegrationEvent, CancellationToken.None));

            // Assert
            Assert.IsTrue(userIntegrationException.Message.Equals("A user with the proposed user id already exists."));
        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_IdentityCreatedIntegrationEventHandler_Due_To_UserName_Already_Existing()
        {
            // Arrange
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            IdentityCreatedIntegrationEvent identityCreatedIntegrationEvent = new()
            {
                UserId = Guid.NewGuid(),
                GivenName = "Glovali",
                UserName = AuthorizedUserName,
                ProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            UserIntegrationException userIntegrationException = await Assert.ThrowsExceptionAsync<UserIntegrationException>(() => Mediator.Send(identityCreatedIntegrationEvent, CancellationToken.None));

            // Assert
            Assert.IsTrue(userIntegrationException.Message.Equals("A user with the proposed user name already exists."));
        }
    }
}
