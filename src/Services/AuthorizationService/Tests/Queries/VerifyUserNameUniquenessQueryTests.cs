using Kwetter.Services.AuthorizationService.API;
using Kwetter.Services.AuthorizationService.API.Application.Queries.VerifyUserNameUniquenessQuery;
using Kwetter.Services.AuthorizationService.API.Controllers;
using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Infrastructure;
using Kwetter.Services.AuthorizationService.Infrastructure.Repositories;
using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.Common.Tests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using XAssert = Xunit.Assert;

namespace Kwetter.Services.AuthorizationService.Tests.Queries
{
    [TestClass]
    public class VerifyUserNameUniquenessQueryTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        public AuthorizationController AuthorizationController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider = InitializeServiceProvider<IdentityDbContext, IdentityDatabaseFactory, IdentityRepository, IdentityAggregate>(typeof(Startup), typeof(VerifyUserNameUniquenessQuery), "AuthorizationService");
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            AuthorizationController = new AuthorizationController(Mediator);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Cleanup(ServiceProvider);
        }

        [TestMethod]
        public async Task Should_Verify_UserName_Uniqueness_Successfully()
        {
            // Arrange
            string userName = "candyman";
            VerifyUserNameUniquenessQuery verifyUserNameUniquenessQuery = new()
            {
                UserName = userName
            };

            // Act
            IActionResult actionResult = await AuthorizationController.VerifyUserNameUniquenessAsync(verifyUserNameUniquenessQuery);

            // Assert
            XAssert.IsType<OkObjectResult>(actionResult);
        }

        [TestMethod]
        public async Task Should_Fail_To_Verify_UserName_Uniqueness_Due_To_Empty_UserName()
        {
            // Arrange
            string userName = "";
            VerifyUserNameUniquenessQuery verifyUserNameUniquenessQuery = new()
            {
                UserName = userName
            };

            // Act
            IActionResult actionResult = await AuthorizationController.VerifyUserNameUniquenessAsync(verifyUserNameUniquenessQuery);

            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            QueryResponse<UserNameUniqueDto> queryResponse = XAssert.IsType<QueryResponse<UserNameUniqueDto>>(badRequestObjectResult.Value);
            XAssert.False(queryResponse.Success);
            XAssert.Contains("The user name can not be empty.", queryResponse.Errors);
        }

        [TestMethod]
        public async Task Should_Fail_To_Verify_UserName_Uniqueness_Due_To_Non_Alphanumeric_UserName()
        {
            // Arrange
            string userName = "dfdf''/-";
            VerifyUserNameUniquenessQuery verifyUserNameUniquenessQuery = new()
            {
                UserName = userName
            };

            // Act
            IActionResult actionResult = await AuthorizationController.VerifyUserNameUniquenessAsync(verifyUserNameUniquenessQuery);

            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            QueryResponse<UserNameUniqueDto> queryResponse = XAssert.IsType<QueryResponse<UserNameUniqueDto>>(badRequestObjectResult.Value);
            XAssert.False(queryResponse.Success);
            XAssert.Contains("The user name is not alphanumeric.", queryResponse.Errors);
        }

        [TestMethod]
        public async Task Should_Fail_To_Verify_UserName_Uniqueness_Due_To_UserName_Exceeding_32_Characters_In_Length()
        {
            // Arrange
            string userName = "verylongusernamethatisveerrrrrrylongreallylong";
            VerifyUserNameUniquenessQuery verifyUserNameUniquenessQuery = new()
            {
                UserName = userName
            };

            // Act
            IActionResult actionResult = await AuthorizationController.VerifyUserNameUniquenessAsync(verifyUserNameUniquenessQuery);

            // Assert
            BadRequestObjectResult badRequestObjectResult = XAssert.IsType<BadRequestObjectResult>(actionResult);
            QueryResponse<UserNameUniqueDto> queryResponse = XAssert.IsType<QueryResponse<UserNameUniqueDto>>(badRequestObjectResult.Value);
            XAssert.False(queryResponse.Success);
            XAssert.Contains("The user name length exceeded the maximum length of 32.", queryResponse.Errors);
        }
    }
}
