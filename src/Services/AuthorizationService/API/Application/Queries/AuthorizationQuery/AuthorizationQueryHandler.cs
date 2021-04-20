using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Infrastructure.Dtos;
using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.AuthorizationQuery
{
    /// <summary>
    /// Represents the <see cref="AuthorizationQueryHandler"/> class.
    /// </summary>
    public sealed class AuthorizationQueryHandler : IRequestHandler<AuthorizationQuery, QueryResponse<AuthorizationDto>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IIdentityRepository _identityRepository;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationQueryHandler"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="identityRepository">The identity repository.</param>
        public AuthorizationQueryHandler(
            IAuthorizationService authorizationService,
            IIdentityRepository identityRepository)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Handles the authorization query.
        /// </summary>
        /// <param name="request">The authorization query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The authorization query response.</returns>
        public async Task<QueryResponse<AuthorizationDto>> Handle(AuthorizationQuery request, CancellationToken cancellationToken)
        {
            QueryResponse<AuthorizationDto> queryResponse = new();
            
            // Performs the authorization_code flow.
            AuthorizationDto authorizationDto = await _authorizationService.AuthorizeAsync(request.Code);

            // According to Google, it is safe to just read the token since we are requesting it from Google.
            JwtSecurityToken token = _jwtSecurityTokenHandler.ReadJwtToken(authorizationDto.IdToken);

            // The open id is the unique identifier within google.
            string openId = token.Subject;

            // Checks if the identity exists within the identity store in the Kwetter platform.
            IdentityAggregate identity = await _identityRepository.FindIdentityByOpenIdAsync(openId, cancellationToken);
            if (identity == default)
            {
                // Otherwise, create the new identity.
                string givenName = token.Claims.First(claim => claim.Type == "given_name").Value;
                string email = token.Claims.First(claim => claim.Type == "email").Value;
                string profilePictureUrl = token.Claims.First(claim => claim.Type == "picture").Value;
                IdentityAggregate newIdentity = new(Guid.NewGuid(), openId, givenName, email, profilePictureUrl);
                identity = _identityRepository.Create(newIdentity);
                await _identityRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            // TODO: maybe update the identity if the picture changed?

            authorizationDto.UserId = identity.Id;
            queryResponse.Data = authorizationDto;
            queryResponse.Success = true;
            return queryResponse;
        }
    }
}
