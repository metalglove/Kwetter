using Kwetter.Services.Common.Application.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Authorization
{
    /// <summary>
    /// Represents the <see cref="JwtTokenAuthenticationHandler"/> class.
    /// </summary>
    public sealed class JwtTokenAuthenticationHandler : AuthenticationHandler<JwtBearerOptions>
    {
        private readonly IConfigurationManager<JsonWebKeySet> _configurationManager;
        private readonly AuthorizationConfiguration _authorizationConfiguration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The jwt bearer options.</param>
        /// <param name="logger">The logger factory.</param>
        /// <param name="encoder">The url encoder.</param>
        /// <param name="clock">The system clock.</param>
        /// <param name="authOptions">The authorization options.</param>
        /// <param name="configurationManager">The configuration manager for the json web key set.</param>
        public JwtTokenAuthenticationHandler(
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<AuthorizationConfiguration> authOptions,
            IConfigurationManager<JsonWebKeySet> configurationManager)
            : base(options, logger, encoder, clock)
        {
            _authorizationConfiguration = authOptions.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _configurationManager = configurationManager;
        }

        /// <inheritdoc cref="AuthenticationHandler{T}.HandleAuthenticateAsync"/>
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
            JsonWebKeySet jsonWebKeySet = await _configurationManager.GetConfigurationAsync(CancellationToken.None);

            TokenValidationParameters validationParameters = new()
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidIssuer = _authorizationConfiguration.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = jsonWebKeySet.Keys,
                ValidateLifetime = true,
                // Allow for some drift in server time
                ClockSkew = TimeSpan.FromMinutes(5),
                ValidAudience = _authorizationConfiguration.ClientId,
                ValidateAudience = true
            };

            List<Claim> claims;
            try
            {
                ClaimsPrincipal claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out SecurityToken rawValidatedToken);
                claims = GetJWTClaims(claimsPrincipal);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Token invalid: {ex.Message}");
            }
            claims.Add(new Claim("token", token));
            ClaimsIdentity identity = new(claims, JwtBearerDefaults.AuthenticationScheme);
            AuthenticationTicket ticket = new(new ClaimsPrincipal(identity), JwtBearerDefaults.AuthenticationScheme);
            return AuthenticateResult.Success(ticket);
        }

        // TODO: decide what kind of claims should be used.
        private List<Claim> GetJWTClaims(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.ToList();
            //Claim claim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.ValueType == "JSON");
            //if (claim == default)
            //    throw new UnauthorizedAccessException("JSON for JWT claim is invalid.");
            //JwtSubject subject = JsonSerializer.Deserialize<JwtSubject>(claim.Value, _jsonSerializerOptions);

            //List<Claim> claims = new()
            //{
            //};

            //foreach (JwtRole role in subject.User.Roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role.Role));
            //    claims.AddRange(role.Capabilities.Select(capability => new Claim("Capability", capability.Capability)));
            //}

            //claims.AddRange(subject.User.Capabilities.Select(jwtCapability => new Claim("Capability", jwtCapability)));
            //return claims;
        }
    }
}
