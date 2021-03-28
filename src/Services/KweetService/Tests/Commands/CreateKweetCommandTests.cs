using System;
using System.Threading.Tasks;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.KweetService.API;
using Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;
using Kwetter.Services.KweetService.Infrastructure;
using Kwetter.Services.KweetService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwetter.Services.KweetService.Tests.Commands
{
    [TestClass]
    public class CreateKweetCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        
        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider = InitializeServices<KweetDbContext, KweetDatabaseFactory, KweetRepository, KweetAggregate>(
                typeof(Startup), typeof(CreateKweetCommand), "KweetService",
                (options, loggerFactory, mediator) => new KweetDatabaseFactory(options, loggerFactory, mediator));
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
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
            CommandResponse response = await Mediator.Send(createKweetCommand);
                
            // Assert
            Assert.IsTrue(response.Success);
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
            CommandResponse response = await Mediator.Send(createKweetCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The user id can not be empty."));
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
            CommandResponse response = await Mediator.Send(createKweetCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The kweet id can not be empty."));
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
            CommandResponse response = await Mediator.Send(createKweetCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The message is null, empty or contains only whitespaces."));
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
            CommandResponse response = await Mediator.Send(createKweetCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The length of the message exceeded 140 characters."));
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
            CommandResponse response = await Mediator.Send(createKweetCommand);
                
            // Assert
            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Contains("The kweet id already exists."));
        }
    }
}