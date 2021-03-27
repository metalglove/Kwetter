using System;
using System.Threading.Tasks;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.FollowService.API;
using Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand;
using Kwetter.Services.FollowService.API.Application.Commands.DeleteFollowCommand;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;
using Kwetter.Services.FollowService.Infrastructure;
using Kwetter.Services.FollowService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwetter.Services.FollowService.Tests.Commands
{
    [TestClass]
    public class DeleteFollowCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        public Guid FollowerId { get; set; }
        public Guid FollowingId { get; set; }
        
        [TestInitialize]
        public async Task Initialize()
        {
            ServiceProvider = InitializeServices<FollowDbContext, FollowDatabaseFactory, FollowRepository, FollowAggregate>(
                typeof(Startup), typeof(CreateFollowCommand), "FollowService",
                (options, loggerFactory, mediator) => new FollowDatabaseFactory(options, loggerFactory, mediator));
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            
            FollowerId = Guid.NewGuid();
            FollowingId = Guid.NewGuid();
            CreateFollowCommand createFollowCommand = new()
            {
                FollowingId = FollowingId,
                FollowerId = FollowerId
            };
            
            // Act
            await Mediator.Send(createFollowCommand);
        }
        
        [TestCleanup]

        public void Cleanup()
        {
            Cleanup(ServiceProvider);    
        }
        
        [TestMethod]
        public async Task Should_Delete_Follow_Through_DeleteFollowCommand()
        {
            // Arrange
            DeleteFollowCommand deleteFollowCommand = new()
            {
                FollowingId = FollowingId,
                FollowerId = FollowerId
            };
            
            // Act
            CommandResponse response = await Mediator.Send(deleteFollowCommand);
                
            // Assert
            Assert.IsTrue(response.Success);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Delete_Follow_Through_DeleteFollowCommand_When_Calling_2nd_Time()
        {
            // Arrange
            DeleteFollowCommand deleteFollowCommand = new()
            {
                FollowingId = FollowingId,
                FollowerId = FollowerId
            };
            
            // Act
            CommandResponse response1 = await Mediator.Send(deleteFollowCommand);
            CommandResponse response2 = await Mediator.Send(deleteFollowCommand);
            
            // Assert
            Assert.IsTrue(response1.Success);
            Assert.IsFalse(response2.Success);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Delete_Follow_Through_DeleteFollowCommand_Due_To_Empty_FollowerId()
        {
            // Arrange
            Guid followerId = Guid.Empty;
            Guid followingId = Guid.NewGuid();
            DeleteFollowCommand deleteFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = followerId
            };
            
            // Act
            CommandResponse response = await Mediator.Send(deleteFollowCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The follower id can not be empty."));
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Delete_Follow_Through_DeleteFollowCommand_Due_To_Empty_FollowingId()
        {
            // Arrange
            Guid followerId = Guid.NewGuid();
            Guid followingId = Guid.Empty;
            DeleteFollowCommand deleteFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = followerId
            };
            
            // Act
            CommandResponse response = await Mediator.Send(deleteFollowCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The following id can not be empty."));
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Delete_Follow_Through_DeleteFollowCommand_Due_To_Follow_Not_Existing()
        {
            // Arrange
            Guid followerId = Guid.NewGuid();
            Guid followingId = Guid.NewGuid();
            DeleteFollowCommand deleteFollowCommand = new()
            {
                FollowingId = followingId,
                FollowerId = followerId
            };
            
            // Act
            CommandResponse response = await Mediator.Send(deleteFollowCommand);    
            
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The follow does not exist."));
        }
    }
}