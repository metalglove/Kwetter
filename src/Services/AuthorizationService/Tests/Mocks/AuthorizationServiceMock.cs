using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.Common.Application.Dtos;
using Kwetter.Services.Common.Tests.Mocks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.Tests.Mocks
{
    internal class AuthorizationServiceMock : TokenVerifierMock, IAuthorizationService
    {
        public AuthorizationServiceMock(ClaimsDto claimsDto) : base(claimsDto)
        {
        }

        public Task SetUserClaimsAsync(string openId, Dictionary<string, object> claims, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
