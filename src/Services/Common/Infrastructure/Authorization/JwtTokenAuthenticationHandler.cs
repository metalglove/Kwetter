using FirebaseAdmin;
using FirebaseAdmin.Auth;
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
        private readonly FirebaseAuth _firebaseAuth;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The jwt bearer options.</param>
        /// <param name="logger">The logger factory.</param>
        /// <param name="encoder">The url encoder.</param>
        /// <param name="clock">The system clock.</param>
        /// <param name="firebaseApp">The firebase app.</param>
        public JwtTokenAuthenticationHandler(
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            FirebaseApp firebaseApp)
            : base(options, logger, encoder, clock)
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp ?? throw new ArgumentNullException(nameof(firebaseApp)));
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
                FirebaseToken decodedToken = await _firebaseAuth.VerifyIdTokenAsync(token);
                string userId = decodedToken.Claims.First(claim => claim.Key == "UserId").Value as string;
                string hasUserClaim = decodedToken.Claims.First(claim => claim.Key == "User").Value.ToString();
                claims.Add(new Claim("UserId", userId));
                claims.Add(new Claim("User", hasUserClaim));
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
