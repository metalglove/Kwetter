using System;
using System.Threading.Tasks;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.FollowService.API;
using Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;
using Kwetter.Services.FollowService.Infrastructure;
using Kwetter.Services.FollowService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwetter.Services.FollowService.Tests.Commands
{
    [TestClass]
    public class CreateFollowCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        
        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider = InitializeServices<FollowDbContext, FollowDatabaseFactory, FollowRepository, FollowAggregate>(
                typeof(Startup), typeof(CreateFollowCommand), "FollowService",
                (options, loggerFactory, mediator) => new FollowDatabaseFactory(options, loggerFactory, mediator));
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
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
            CommandResponse response = await Mediator.Send(createFollowCommand);
                
            // Assert
            Assert.IsTrue(response.Success);
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
            CommandResponse response = await Mediator.Send(createFollowCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The follower id can not be empty."));
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
            CommandResponse response = await Mediator.Send(createFollowCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The following id can not be empty."));
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
            CommandResponse response = await Mediator.Send(createFollowCommand);    
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The follow already exists."));
        }
    }
}