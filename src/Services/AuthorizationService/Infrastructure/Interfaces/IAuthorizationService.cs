using Kwetter.Services.Common.Application.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IAuthorizationService"/> interface.
    /// </summary>
    public interface IAuthorizationService : ITokenVerifier
    {
        /// <summary>
        /// Sets user claims aysynchronously.
        /// </summary>
        /// <param name="openId">The open id.</param>
        /// <param name="claims">The claims dictionary.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns an awaitable task.</returns>
        public Task SetUserClaimsAsync(string openId, Dictionary<string, object> claims, CancellationToken cancellationToken);
    }
}
