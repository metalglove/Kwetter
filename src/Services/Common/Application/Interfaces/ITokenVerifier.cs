using Kwetter.Services.Common.Application.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="ITokenVerifier"/> interface.
    /// </summary>
    public interface ITokenVerifier
    {
        /// <summary>
        /// Verifies the id token asynchronously.
        /// </summary>
        /// <param name="idToken">The id token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task<ClaimsDto> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken = default);
    }
}
