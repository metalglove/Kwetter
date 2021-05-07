using Kwetter.Services.Common.Application.Dtos;
using Kwetter.Services.Common.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Tests.Mocks
{
    public class TokenVerifierMock : ITokenVerifier
    {
        protected readonly ClaimsDto _claimsDto;

        public TokenVerifierMock(ClaimsDto claimsDto)
        {
            _claimsDto = claimsDto;
        }

        public Task<ClaimsDto> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_claimsDto);
        }
    }
}
