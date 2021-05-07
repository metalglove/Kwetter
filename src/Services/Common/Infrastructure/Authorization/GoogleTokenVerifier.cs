using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Kwetter.Services.Common.Application.Dtos;
using Kwetter.Services.Common.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Authorization
{
    /// <summary>
    /// Represents the <see cref="GoogleTokenVerifier"/> class.
    /// </summary>
    public class GoogleTokenVerifier : ITokenVerifier
    {
        protected readonly FirebaseAuth _firebaseAuth;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleTokenVerifier"/> class.
        /// </summary>
        /// <param name="firebaseApp">The firebase app.</param>
        public GoogleTokenVerifier(FirebaseApp firebaseApp)
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp ?? throw new ArgumentNullException(nameof(firebaseApp)));
        }

        /// <inheritdoc cref="ITokenVerifier.VerifyIdTokenAsync(string, CancellationToken)"/>
        public async Task<ClaimsDto> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken = default)
        {
            FirebaseToken decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken, cancellationToken);
            return new ClaimsDto
            {
                Subject = decodedToken.Subject,
                Audience = decodedToken.Audience,
                Issuer = decodedToken.Issuer,
                ExpirationTimeSeconds = decodedToken.ExpirationTimeSeconds,
                IssuedAtTimeSeconds = decodedToken.IssuedAtTimeSeconds,
                Claims = new Dictionary<string, ClaimDto>(decodedToken.Claims
                            .Select(kvp => new KeyValuePair<string, ClaimDto>(kvp.Key, new ClaimDto() { Name = kvp.Key, Value = kvp.Value as string })))
            };
        }
    }
}
