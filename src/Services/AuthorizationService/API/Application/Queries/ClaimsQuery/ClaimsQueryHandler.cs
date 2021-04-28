using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Infrastructure.Dtos;
using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.ClaimsQuery
{
    /// <summary>
    /// Represents the <see cref="ClaimsQueryHandler"/> class.
    /// </summary>
    public sealed class ClaimsQueryHandler : IRequestHandler<ClaimsQuery, QueryResponse<ClaimsDto>>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly FirebaseAuth _firebaseAuth;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsQueryHandler"/> class.
        /// </summary>
        /// <param name="identityRepository">The identity repository.</param>
        /// <param name="firebaseApp">The firebase app.</param>
        public ClaimsQueryHandler(
            IIdentityRepository identityRepository,
            FirebaseApp firebaseApp)
        {
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp ?? throw new ArgumentNullException(nameof(firebaseApp)));
        }

        /// <summary>
        /// Handles the claims query.
        /// </summary>
        /// <param name="request">The claims query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The claims query response.</returns>
        public async Task<QueryResponse<ClaimsDto>> Handle(ClaimsQuery request, CancellationToken cancellationToken)
        {
            QueryResponse<ClaimsDto> queryResponse = new();
            queryResponse.Data = new ClaimsDto();

            await _firebaseAuth.VerifyIdTokenAsync(request.IdToken);

            // According to Google, it is safe to just read the token since we are requesting it from Google.
            FirebaseToken token = await _firebaseAuth.VerifyIdTokenAsync(request.IdToken);

            // The open id is the unique identifier within google.
            string openId = token.Subject;

            // Checks if the identity exists within the identity store in the Kwetter platform.
            IdentityAggregate identity = await _identityRepository.FindIdentityByOpenIdAsync(openId, cancellationToken);
            if (identity == default)
            {
                // Otherwise, create the new identity.
                string givenName = token.Claims.First(claim => claim.Key == "name").Value as string;
                string email = token.Claims.First(claim => claim.Key == "email").Value as string;
                string profilePictureUrl = token.Claims.First(claim => claim.Key == "picture").Value as string;
                IdentityAggregate newIdentity = new(Guid.NewGuid(), openId, givenName, email, profilePictureUrl);
                string uid = newIdentity.Id.ToString();
                Dictionary<string, object> claims = new()
                {
                    { "UserId", uid },
                    { "User", true }
                };
                await _firebaseAuth.SetCustomUserClaimsAsync(openId, claims, cancellationToken);
                queryResponse.Data.IdToken = await _firebaseAuth.CreateCustomTokenAsync(uid, claims, cancellationToken);
                queryResponse.Data.Claims = claims.Select((kvp) => new ClaimDto() { Type = kvp.Key, Value = kvp.Value as string }).ToList();
                identity = _identityRepository.Create(newIdentity);
                await _identityRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            // TODO: maybe update the identity if the picture changed?
            queryResponse.Data.UserId = identity.Id;
            queryResponse.Success = true;
            return queryResponse;
        }
    }
}
