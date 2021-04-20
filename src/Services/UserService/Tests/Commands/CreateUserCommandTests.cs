using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.UserService.API;
using Kwetter.Services.UserService.API.Application.Commands.CreateUserCommand;
using Kwetter.Services.UserService.API.Controllers;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.UserService.Infrastructure;
using Kwetter.Services.UserService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using XAssert = Xunit.Assert;

namespace Kwetter.Services.UserService.Tests.Commands
{
    [TestClass]
    public class CreateUserCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        public UserController UserController { get; set; }
        public Guid UserId { get; set; }

        [TestInitialize]
        public async Task Initialize()
        {
            ServiceProvider = InitializeServices<UserDbContext, UserDatabaseFactory, UserRepository, UserAggregate>(typeof(Startup), typeof(CreateUserCommand), "UserService");
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            UserController = new UserController(Mediator);
            UserId = Guid.NewGuid();
            CreateUserCommand createUserCommand = new()
            {
                UserId = UserId,
                UserDisplayName = "AlreadyExistingUser",
                UserProfileDescription = "My profile description!",
                UserProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg"
            };

            await UserController.CreateAsync(createUserCommand);
        }

        [TestCleanup]

        public void Cleanup()
        {
            Cleanup(ServiceProvider);
        }

        [TestMethod]
        public async Task Should_Create_User_Through_CreateUserCommand()
        {
            // Arrange
            CreateUserCommand createUserCommand = new()
            {
                UserId = Guid.NewGuid(),
                UserDisplayName = "Glovali",
                UserProfileDescription = "My profile description!",
                UserProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg"
            };

            // Act
            IActionResult actionResult = await UserController.CreateAsync(createUserCommand);

            // Assert
            CreatedAtRouteResult createdAtRouteResult = XAssert.IsType<CreatedAtRouteResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(createdAtRouteResult.Value);
            XAssert.True(commandResponse.Success);
            XAssert.True(commandResponse.Errors.Count == 0);
        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_CreateUserCommand_Due_To_Empty_UserId()
        {
            // Arrange
            Guid userId = Guid.Empty;
            const string userDisplayName = "Glovali";
            const string userProfileDescription = "Hello world!";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            CreateUserCommand createUserCommand = new()
            {
                UserId = userId,
                UserDisplayName = userDisplayName,
                UserProfileDescription = userProfileDescription,
                UserProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            IActionResult actionResult = await UserController.CreateAsync(createUserCommand);

            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The user id can not be empty.", commandResponse.Errors);
        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_CreateUserCommand_Due_To_Empty_DisplayName()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "";
            const string userProfileDescription = "Hello world!";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            CreateUserCommand createUserCommand = new()
            {
                UserId = userId,
                UserDisplayName = userDisplayName,
                UserProfileDescription = userProfileDescription,
                UserProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            IActionResult actionResult = await UserController.CreateAsync(createUserCommand);

            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The display name must not be null or empty.", commandResponse.Errors);
        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_CreateUserCommand_Due_To_DisplayName_Exceeding_64_Length()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            const string userDisplayName = "sdfdsfsfsdfdsfdsfdfdsfdsfdsfdsfdsfdsfdsfdfdfdfdfdfdfdfddfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdffdfdfdff";
            const string userProfileDescription = "Hello world!";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            CreateUserCommand createUserCommand = new()
            {
                UserId = userId,
                UserDisplayName = userDisplayName,
                UserProfileDescription = userProfileDescription,
                UserProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            IActionResult actionResult = await UserController.CreateAsync(createUserCommand);

            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The display name must not exceed 64 characters.", commandResponse.Errors);
        }

        [TestMethod]
        public async Task Should_Fail_To_Create_User_Through_CreateUserCommand_Due_To_UserId_Already_Existing()
        {
            // Arrange
            const string userDisplayName = "AlreadyExistingUser";
            const string userProfileDescription = "Hello world!";
            const string userProfilePictureUrl = "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg";
            CreateUserCommand createUserCommand = new()
            {
                UserId = UserId,
                UserDisplayName = userDisplayName,
                UserProfileDescription = userProfileDescription,
                UserProfilePictureUrl = userProfilePictureUrl
            };

            // Act
            IActionResult actionResult = await UserController.CreateAsync(createUserCommand);

            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("A user with the proposed user id already exists.", commandResponse.Errors);
        }
    }
}
