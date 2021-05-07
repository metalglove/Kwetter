using FirebaseAdmin;
using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.Common.Infrastructure.Authorization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="GoogleAuthorizationService"/> class.
    /// </summary>
    public class GoogleAuthorizationService : GoogleTokenVerifier, IAuthorizationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAuthorizationService"/> class.
        /// </summary>
        /// <param name="firebaseApp">The firebase app.</param>
        public GoogleAuthorizationService(FirebaseApp firebaseApp) : base(firebaseApp)
        {

        }

        /// <inheritdoc cref="IAuthorizationService.SetUserClaimsAsync(string, Dictionary{string, object}, CancellationToken)"/>
        public Task SetUserClaimsAsync(string openId, Dictionary<string, object> claims, CancellationToken cancellationToken)
        {
            return _firebaseAuth.SetCustomUserClaimsAsync(openId, claims, cancellationToken);
        }
    }
}
