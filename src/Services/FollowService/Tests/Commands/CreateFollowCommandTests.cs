using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.FollowService.API;
using Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand;
using Kwetter.Services.FollowService.API.Controllers;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;
using Kwetter.Services.FollowService.Infrastructure;
using Kwetter.Services.FollowService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using XAssert = Xunit.Assert;

namespace Kwetter.Services.FollowService.Tests.Commands
{
    [TestClass]
    public class CreateFollowCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        public FollowController FollowController { get; set; }
        
        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider = InitializeServices<FollowDbContext, FollowDatabaseFactory, FollowRepository, FollowAggregate>(typeof(Startup), typeof(CreateFollowCommand), "FollowService");
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            FollowController = new FollowController(Mediator);
        }
        
        [TestCleanup]

        public void Cleanup()
        {
            Cleanup(ServiceProvider);    
        }

        [TestMethod]
        public async Task Should_Create_Follow_Through_CreateFollowCommand()
        {
            // Arrange
            Guid followerId = Guid.NewGuid();
            Guid followingId = Guid.NewGuid();
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = followerId
            };
            
            // Act
            IActionResult actionResult = await FollowController.CreateAsync(createFollowCommand);
            
            // Assert
            CreatedAtRouteResult createdAtRouteResult = XAssert.IsType<CreatedAtRouteResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(createdAtRouteResult.Value);
            XAssert.True(commandResponse.Success);
            XAssert.True(commandResponse.Errors.Count == 0);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Follow_Through_CreateFollowCommand_Due_To_Empty_FollowerId()
        {
            // Arrange
            Guid followerId = Guid.Empty;
            Guid followingId = Guid.NewGuid();
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = followerId
            };
            
            // Act
            IActionResult actionResult = await FollowController.CreateAsync(createFollowCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The follower id can not be empty.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Follow_Through_CreateFollowCommand_Due_To_Empty_FollowingId()
        {
            // Arrange
            Guid followerId = Guid.NewGuid();
            Guid followingId = Guid.Empty;
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = followerId
            };
            
            // Act
            IActionResult actionResult = await FollowController.CreateAsync(createFollowCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The following id can not be empty.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Follow_Through_CreateFollowCommand_Due_To_Follow_Already_Existing()
        {
            // Arrange
            Guid followerId = Guid.NewGuid();
            Guid followingId = Guid.NewGuid();
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = followerId
            };
            CommandResponse _ = await Mediator.Send(createFollowCommand);
            
            // Act
            IActionResult actionResult = await FollowController.CreateAsync(createFollowCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The follow already exists.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Follow_Through_CreateFollowCommand_Due_To_Same_Follower_And_Following_Ids()
        {
            // Arrange
            Guid followerId = Guid.NewGuid();
            Guid followingId = followerId;
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = followerId
            };

            // Act
            IActionResult actionResult = await FollowController.CreateAsync(createFollowCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The follow and following id are the same. One can not follow themself.", commandResponse.Errors);
        }
    }
}