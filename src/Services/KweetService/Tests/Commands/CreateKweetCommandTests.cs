using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.KweetService.API;
using Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand;
using Kwetter.Services.KweetService.API.Controllers;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;
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
        public void Initialize()
        {
            ServiceProvider = InitializeServices<KweetDbContext, KweetDatabaseFactory, KweetRepository, KweetAggregate>(typeof(Startup), typeof(CreateKweetCommand), "KweetService");
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            KweetController = new KweetController(Mediator);
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
            Guid userId = Guid.NewGuid();
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
            CreatedAtRouteResult createdAtRouteResult = XAssert.IsType<CreatedAtRouteResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(createdAtRouteResult.Value);
            XAssert.True(commandResponse.Success);
            XAssert.True(commandResponse.Errors.Count == 0);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_Empty_UserId()
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
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The user id can not be empty.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_Empty_KweetId()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid kweetId = Guid.Empty;
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
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet id can not be empty.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_Message_Being_Empty()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid kweetId = Guid.NewGuid();
            const string message = "";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = userId,
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
            Guid userId = Guid.NewGuid();
            Guid kweetId = Guid.NewGuid();
            const string message = "sfafdsafadsffsaddsfasdfdsfdsfsfafdsafadsffsaddsfasdfdsfdsfsfafdsafadsffsaddsfasdfdsfdsfsfafdsafadsffsaddsfasdfdsfdddsfsdsfdssddssddsdsdsdsdsf";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = userId,
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
        public async Task Should_Fail_To_Create_Kweet_Through_CreateKweetCommand_Due_To_MessageId_Already_Existing()
        {
            // Arrange
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid kweetId = Guid.NewGuid();
            const string message = "Hello world!";
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = userId,
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