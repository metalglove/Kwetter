using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.KweetService.API;
using Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand;
using Kwetter.Services.KweetService.API.Controllers;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.KweetService.Infrastructure;
using Kwetter.Services.KweetService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using XAssert = Xunit.Assert;

namespace Kwetter.Services.KweetService.Tests.Commands
{
    [TestClass]
    public class CreateKweetCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        public KweetController KweetController { get; set; }
        
        [TestInitialize]
        public async Task Initialize()
        {
            ServiceProvider = InitializeServiceProvider<KweetDbContext, KweetDatabaseFactory, UserRepository, UserAggregate>(typeof(Startup), typeof(CreateKweetCommand), "KweetService");
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            KweetController = CreateAuthorizedController<KweetController>(Mediator);
            IUserRepository userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            UserAggregate user = new(AuthorizedUserId, "Glovali", AuthorizedUserName, "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            userRepository.Create(user);
            await userRepository.UnitOfWork.SaveChangesAsync();
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            Cleanup(ServiceProvider);    
        }

        [TestMethod]
        public async Task Should_Create_Kweet_Through_CreateKweetCommand()
        {
            // Arrange
            Guid kweetId = Guid.NewGuid();
            const string message = "Hello world!";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = kweetId,
                Message = message
            };
            
            // Act
            IActionResult actionResult = await KweetController.CreateAsync(createKweetCommand);
            
            // Assert
            CreatedAtRouteResult createdAtRouteResult = XAssert.IsType<CreatedAtRouteResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(createdAtRouteResult.Value);
            XAssert.True(commandResponse.Success);
            XAssert.True(commandResponse.Errors.Count == 0);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_Unauthorized()
        {
            // Arrange
            Guid userId = Guid.Empty;
            Guid kweetId = Guid.NewGuid();
            const string message = "Hello world!";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = userId,
                KweetId = kweetId,
                Message = message
            };

            // Act
            IActionResult actionResult = await KweetController.CreateAsync(createKweetCommand);

            // Assert
            UnauthorizedObjectResult unauthorizedObjectResult = XAssert.IsType<UnauthorizedObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(unauthorizedObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The user id claim and provided user id are not the same.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_Empty_KweetId()
        {
            // Arrange
            Guid kweetId = Guid.Empty;
            const string message = "Hello world!";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = kweetId,
                Message = message
            };
            
            // Act
            IActionResult actionResult = await KweetController.CreateAsync(createKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet id can not be empty.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_Message_Being_Empty()
        {
            // Arrange
            Guid kweetId = Guid.NewGuid();
            const string message = "";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = kweetId,
                Message = message
            };
            
            // Act
            IActionResult actionResult = await KweetController.CreateAsync(createKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The message is null, empty or contains only whitespaces.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_Message_Exceeding_140_Length()
        {
            // Arrange
            Guid kweetId = Guid.NewGuid();
            const string message = "sfafdsafadsffsaddsfasdfdsfdsfsfafdsafadsffsaddsfasdfdsfdsfsfafdsafadsffsaddsfasdfdsfdsfsfafdsafadsffsaddsfasdfdsfdddsfsdsfdssddssddsdsdsdsdsf";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = kweetId,
                Message = message
            };
            
            // Act
            IActionResult actionResult = await KweetController.CreateAsync(createKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The length of the message exceeded 140 characters.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_KweetId_Already_Existing()
        {
            // Arrange
            Guid kweetId = Guid.NewGuid();
            const string message = "Hello world!";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = kweetId,
                Message = message
            };
            CommandResponse _ = await Mediator.Send(createKweetCommand);
            
            // Act
            IActionResult actionResult = await KweetController.CreateAsync(createKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet id already exists.", commandResponse.Errors);
        }
    }
}