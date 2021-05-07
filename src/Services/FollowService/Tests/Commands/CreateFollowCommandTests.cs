using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.FollowService.API;
using Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand;
using Kwetter.Services.FollowService.API.Controllers;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
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
        public Guid FollowerId { get; set; }
        public Guid FollowingId { get; set; }
        public FollowController FollowController { get; set; }

        [TestInitialize]
        public async Task Initialize()
        {
            ServiceProvider = InitializeServiceProvider<FollowDbContext, FollowDatabaseFactory, UserRepository, UserAggregate>(typeof(Startup), typeof(CreateFollowCommand), "FollowService");
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            FollowerId = AuthorizedUserId;
            FollowingId = Guid.NewGuid();

            FollowController = CreateAuthorizedController<FollowController>(Mediator);

            IUserRepository userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            UserAggregate follower = new(FollowerId, "SuperMario", "supermario", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            userRepository.Create(follower);

            UserAggregate following = new(FollowingId, "candyman67", "candyman", "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            userRepository.Create(following);
            await userRepository.UnitOfWork.SaveEntitiesAsync();
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
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = FollowingId,
                FollowerId = FollowerId
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
            UnauthorizedObjectResult unauthorizedObjectResult = XAssert.IsType<UnauthorizedObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(unauthorizedObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The user id and follower id are not the same.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Create_Follow_Through_CreateFollowCommand_Due_To_Empty_FollowingId()
        {
            // Arrange
            Guid followingId = Guid.Empty;
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = FollowerId
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
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = FollowingId,
                FollowerId = FollowerId
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
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = FollowerId,
                FollowerId = FollowerId
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