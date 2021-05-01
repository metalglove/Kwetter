using Kwetter.Services.Common.Application.Dtos;
using Kwetter.Services.Common.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Authorization
{
    /// <summary>
    /// Represents the <see cref="JwtTokenAuthenticationHandler"/> class.
    /// </summary>
    public sealed class JwtTokenAuthenticationHandler : AuthenticationHandler<JwtBearerOptions>
    {
        private readonly ITokenVerifier _tokenVerifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The jwt bearer options.</param>
        /// <param name="logger">The logger factory.</param>
        /// <param name="encoder">The url encoder.</param>
        /// <param name="clock">The system clock.</param>
        /// <param name="tokenVerifier">The token verifier.</param>
        public JwtTokenAuthenticationHandler(
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenVerifier tokenVerifier)
            : base(options, logger, encoder, clock)
        {
            _tokenVerifier = tokenVerifier ?? throw new ArgumentNullException(nameof(tokenVerifier));
        }

        /// <inheritdoc cref="AuthenticationHandler{T}.HandleAuthenticateAsync()"/>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            IHeaderDictionary headers = Request.Headers;
            if (!headers.ContainsKey("Authorization"))
            {
                // TODO: fix random requests like /favicon.ico
                Endpoint endPoint = Request.HttpContext.GetEndpoint();
                if (endPoint is not null && endPoint.Metadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
                {
                    ClaimsIdentity id = new(new List<Claim>(), JwtBearerDefaults.AuthenticationScheme);
                    AuthenticationTicket tick = new(new ClaimsPrincipal(id), JwtBearerDefaults.AuthenticationScheme);
                    return AuthenticateResult.Success(tick);
                }
                return AuthenticateResult.Fail($"Authorization header not found.");
            }

            string token = headers["Authorization"].ToString().Replace("Bearer ", "");
            List<Claim> claims = new();
            try
            {
                ClaimsDto claimsDto = await _tokenVerifier.VerifyIdTokenAsync(token);
                claims.Add(claimsDto.Claims["UserId"].ToClaim());
                claims.Add(claimsDto.Claims["User"].ToClaim());
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Token invalid: {ex.Message}");
            }
            ClaimsIdentity identity = new(claims, JwtBearerDefaults.AuthenticationScheme);
            AuthenticationTicket ticket = new(new ClaimsPrincipal(identity), JwtBearerDefaults.AuthenticationScheme);
            return AuthenticateResult.Success(ticket);
        }
    }
}
