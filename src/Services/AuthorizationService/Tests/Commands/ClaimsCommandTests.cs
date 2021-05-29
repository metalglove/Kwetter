using Kwetter.Services.AuthorizationService.API;
using Kwetter.Services.AuthorizationService.API.Application.Commands.ClaimsCommand;
using Kwetter.Services.AuthorizationService.API.Controllers;
using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Infrastructure;
using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.AuthorizationService.Infrastructure.Repositories;
using Kwetter.Services.AuthorizationService.Tests.Mocks;
using Kwetter.Services.Common.Infrastructure.Authorization;
using Kwetter.Services.Common.Tests;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace Kwetter.Services.AuthorizationService.Tests.Commands
{
    [TestClass]
    public class ClaimsCommandTests : TestBase
    {
        public ServiceProvider ServiceProvider { get; set; }
        public IMediator Mediator { get; set; }
        public AuthorizationController AuthorizationController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            IServiceCollection services = InitializeServices<IdentityDbContext, IdentityDatabaseFactory, IdentityRepository, IdentityAggregate>(typeof(Startup), typeof(ClaimsCommand), "AuthorizationService");
            services.AddSingleton<IAuthorizationService>((a) =>
            {
                return new AuthorizationServiceMock(ClaimsDto);
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtBearerOptions, JwtTokenAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, o => { });

            ServiceProvider = services.BuildServiceProvider();
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            AuthorizationController = CreateAuthorizedController<AuthorizationController>(Mediator);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Cleanup(ServiceProvider);
        }

        //[TestMethod]
        //public async Task Should_Fail_To_Set_Claims_Due_To_Unauthorized_User()
        //{
        //    // Arrange
        //    string userName = "candyman";
        //    string idToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImNjM2Y0ZThiMmYxZDAyZjBlYTRiMWJkZGU1NWFkZDhiMDhiYzUzODYiLCJ0eXAiOiJKV1QifQ.eyJuYW1lIjoidGVzdCB1c2VyIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FBVFhBSngwcVVkZmJHSmxjaUNXcGZqb1pXSkRXeUl2OW8yVkFFcjBya3BhPXM5Ni1jIiwiVXNlcklkIjoiODM2YjRkMDktYjBlMy00ODE0LWFiZTktY2QwMzZmNjhmMTRmIiwiVXNlck5hbWUiOiJrd2V0dGVybWFuIiwiVXNlciI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL3M2NC0xLXZldGlzIiwiYXVkIjoiczY0LTEtdmV0aXMiLCJhdXRoX3RpbWUiOjE2MjAyNTI5NTEsInVzZXJfaWQiOiIxOE9OQ2VpSlp4WjVENHJtSmdBWEVraVJWb3IyIiwic3ViIjoiMThPTkNlaUpaeFo1RDRybUpnQVhFa2lSVm9yMiIsImlhdCI6MTYyMDI1Mjk1MiwiZXhwIjoxNjIwMjU2NTUyLCJlbWFpbCI6Imt3ZXR0ZXJ1c2VyQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7Imdvb2dsZS5jb20iOlsiMTA0NTE3MzYzNTY0NzY4NzQ5OTQ5Il0sImVtYWlsIjpbImt3ZXR0ZXJ1c2VyQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6Imdvb2dsZS5jb20ifX0.gg9ylhFG5XFLMqKSAqYybGeDpsflpbLPCXyEsOTc1o9PdbBye3C1CMxfJr7zF_TmCGk9SOOqjYhd3gH8bVrCXTN3dlIOPJ6ElSv5JXcKHT6VwW76odWaE10OHhqOcQf7Xh1PYl6Zy8NiKH9eiW70NLBLlDvPESFsvS-PIn9eKnN2scKllN3fG9xseMod9VUdikkb89Vjzrx1Ad1oSuSbQXNAykIri2-ek8Y5eQg44BWfYHB3_SSIlVeIm1UO3x0VbmRHgeNtniYipzXa3EyQ4KmKFppeN6O8hIGh1Vr1wNJ87WuMNOSJqjUViQF1Hrww4mviAnWPP6dSdx63GZ0Xdw";
        //    ClaimsCommand claimsCommand = new()
        //    {
        //        UserName = userName,
        //        IdToken = idToken
        //    };

        //    // Act
        //    AuthorizationController authorizationController = new(Mediator);
        //    IActionResult actionResult = await authorizationController.ClaimsAsync(claimsCommand);

        //    // Assert
        //    Assert.IsType<UnauthorizedObjectResult>(actionResult);
        //}
    }
}
