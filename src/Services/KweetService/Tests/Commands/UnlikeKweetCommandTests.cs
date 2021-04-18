using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.KweetService.API;
using Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand;
using Kwetter.Services.KweetService.API.Application.Commands.LikeKweetCommand;
using Kwetter.Services.KweetService.API.Application.Commands.UnlikeKweetCommand;
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
    public class UnlikeKweetCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        public Guid UserId { get; set; }
        public Guid KweetId { get; set; }
        public KweetController KweetController { get; set; }

        [TestInitialize]
        public async Task Initialize()
        {
            ServiceProvider = InitializeServices<KweetDbContext, KweetDatabaseFactory, KweetRepository, KweetAggregate>(
                typeof(Startup), typeof(CreateKweetCommand), "KweetService",
                (options, loggerFactory, mediator) => new KweetDatabaseFactory(options, loggerFactory, mediator));
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            KweetController = new KweetController(Mediator);
            
            UserId = Guid.NewGuid();
            KweetId = Guid.NewGuid();
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = UserId,
                KweetId = KweetId,
                Message = "Hello world!"
            };
            await Mediator.Send(createKweetCommand);
            LikeKweetCommand likeKweetCommand = new()
            {
                UserId = UserId,
                KweetId = KweetId
            };
            await Mediator.Send(likeKweetCommand);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Cleanup(ServiceProvider);
        }
        
        [TestMethod]
        public async Task Should_Unlike_Kweet_Through_UnlikeKweetCommand()
        {
            // Arrange
            UnlikeKweetCommand unlikeKweetCommand = new()
            {
                UserId = UserId,
                KweetId = KweetId
            };
            
            // Act
            IActionResult actionResult = await KweetController.UnlikeAsync(unlikeKweetCommand);
            
            // Assert
            XAssert.IsType<OkObjectResult>(actionResult);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Unlike_Kweet_Through_UnlikeKweetCommand_Due_To_Empty_UserId()
        {
            // Arrange
            UnlikeKweetCommand unlikeKweetCommand = new()
            {
                UserId = Guid.Empty,
                KweetId = KweetId
            };
            
            // Act
            IActionResult actionResult = await KweetController.UnlikeAsync(unlikeKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The user id can not be empty.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Unlike_Kweet_Through_UnlikeKweetCommand_Due_To_Empty_KweetId()
        {
            // Arrange
            UnlikeKweetCommand unlikeKweetCommand = new()
            {
                UserId = UserId,
                KweetId = Guid.Empty
            };
            
            // Act
            IActionResult actionResult = await KweetController.UnlikeAsync(unlikeKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet id can not be empty.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Unlike_Kweet_Through_UnlikeKweetCommand_Due_To_Kweet_Not_Existing()
        {
            // Arrange
            UnlikeKweetCommand unlikeKweetCommand = new()
            {
                UserId = UserId,
                KweetId = Guid.NewGuid()
            };
            
            // Act
            IActionResult actionResult = await KweetController.UnlikeAsync(unlikeKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet does not exist.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Unlike_Kweet_Through_UnlikeKweetCommand_Due_To_Kweet_Not_Being_Liked()
        {
            // Arrange
            UnlikeKweetCommand unlikeKweetCommand = new()
            {
                UserId = UserId,
                KweetId = KweetId
            };
            CommandResponse _ = await Mediator.Send(unlikeKweetCommand);

            // Act
            IActionResult actionResult = await KweetController.UnlikeAsync(unlikeKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet was not liked.", commandResponse.Errors);
        }
    }
}