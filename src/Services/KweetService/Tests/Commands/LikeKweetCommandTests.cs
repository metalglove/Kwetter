using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.Common.Tests;
using Kwetter.Services.KweetService.API;
using Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand;
using Kwetter.Services.KweetService.API.Application.Commands.LikeKweetCommand;
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
    public class LikeKweetCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        public Guid KweetId { get; set; }
        public KweetController KweetController { get; set; }
        
        [TestInitialize]
        public async Task Initialize()
        {
            ServiceProvider = InitializeServiceProvider<KweetDbContext, KweetDatabaseFactory, UserRepository, UserAggregate>(typeof(Startup), typeof(CreateKweetCommand), "KweetService");
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            KweetController = CreateAuthorizedController<KweetController>(Mediator);
            IUserRepository userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            UserAggregate user = new(AuthorizedUserId, "kwetter user", AuthorizedUserName, "https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg");
            userRepository.Create(user);
            await userRepository.UnitOfWork.SaveChangesAsync();
            KweetId = Guid.NewGuid();
            CreateKweetCommand createKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = KweetId,
                Message = "Hello world!"
            };
            await Mediator.Send(createKweetCommand);
        }
        
        [TestCleanup]

        public void Cleanup()
        {
            Cleanup(ServiceProvider);    
        }
        
        [TestMethod]
        public async Task Should_Like_Kweet_Through_LikeKweetCommand()
        {
            // Arrange
            LikeKweetCommand likeKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = KweetId
            };
            
            // Act
            IActionResult actionResult = await KweetController.LikeAsync(likeKweetCommand);
            
            // Assert
            XAssert.IsType<OkObjectResult>(actionResult);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Like_Kweet_Through_LikeKweetCommand_Due_To_Unauthorized()
        {
            // Arrange
            LikeKweetCommand likeKweetCommand = new()
            {
                UserId = Guid.Empty,
                KweetId = KweetId
            };
                
            // Act
            IActionResult actionResult = await KweetController.LikeAsync(likeKweetCommand);

            // Assert
            UnauthorizedObjectResult unauthorizedObjectResult = XAssert.IsType<UnauthorizedObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(unauthorizedObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The user id claim and provided user id are not the same.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Like_Kweet_Through_LikeKweetCommand_Due_To_Empty_KweetId()
        {
            // Arrange
            LikeKweetCommand likeKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = Guid.Empty
            };
            
            // Act
            IActionResult actionResult = await KweetController.LikeAsync(likeKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet id can not be empty.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Like_Kweet_Through_LikeKweetCommand_Due_To_Kweet_Not_Existing()
        {
            // Arrange
            LikeKweetCommand likeKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = Guid.NewGuid()
            };
            
            // Act
            IActionResult actionResult = await KweetController.LikeAsync(likeKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet does not exist.", commandResponse.Errors);
        }
        
        [TestMethod]
        public async Task Should_Fail_To_Like_Kweet_Through_LikeKweetCommand_Due_To_Kweet_Already_Being_Liked()
        {
            // Arrange
            LikeKweetCommand likeKweetCommand = new()
            {
                UserId = AuthorizedUserId,
                KweetId = KweetId
            };
            CommandResponse _ = await Mediator.Send(likeKweetCommand);
            
            // Act
            IActionResult actionResult = await KweetController.LikeAsync(likeKweetCommand);
            
            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            CommandResponse commandResponse = XAssert.IsType<CommandResponse>(badRequestObjectResult.Value);
            XAssert.False(commandResponse.Success);
            XAssert.Contains("The kweet is already liked.", commandResponse.Errors);
        }
    }
}