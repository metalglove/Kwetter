using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Infrastructure.Interfaces;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.API.Application.Commands.ClaimsCommand
{
    /// <summary>
    /// Represents the <see cref="ClaimsCommandHandler"/> class.
    /// </summary>
    public sealed class ClaimsCommandHandler : IRequestHandler<ClaimsCommand, CommandResponse>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsCommandHandler"/> class.
        /// </summary>
        /// <param name="identityRepository">The identity repository.</param>
        /// <param name="authorizationService">The authorization service.</param>
        public ClaimsCommandHandler(
            IIdentityRepository identityRepository,
            IAuthorizationService authorizationService)
        {
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        /// <summary>
        /// Handles the claims command.
        /// </summary>
        /// <param name="request">The claims command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The command response.</returns>
        public async Task<CommandResponse> Handle(ClaimsCommand request, CancellationToken cancellationToken)
        {
            CommandResponse commandResponse = new();

            // According to Google, it is safe to just read the token since we are requesting it from Google.
            ClaimsDto claimsDto = await _authorizationService.VerifyIdTokenAsync(request.IdToken, cancellationToken);

            // The open id is the unique identifier within google.
            string openId = claimsDto.Subject;

            // Checks if the identity exists within the identity store in the Kwetter platform.
            IdentityAggregate identity = await _identityRepository.FindIdentityByOpenIdAsync(openId, cancellationToken);
            if (identity == default)
            {
                // Otherwise, create the new identity.
                string givenName = claimsDto.Claims["name"].Value;
                string email = claimsDto.Claims["email"].Value;
                string profilePictureUrl = claimsDto.Claims["picture"].Value;
                IdentityAggregate newIdentity = new(Guid.NewGuid(), openId, givenName, email, profilePictureUrl);
                string uid = newIdentity.Id.ToString();
                Dictionary<string, object> claims = new()
                {
                    { "UserId", uid },
                    { "User", true }
                };
                await _authorizationService.SetUserClaimsAsync(openId, claims, cancellationToken);
                identity = _identityRepository.Create(newIdentity);
                await _identityRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            commandResponse.Success = identity != default;
            return commandResponse;
        }
    }
}
