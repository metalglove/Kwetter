namespace Kwetter.Services.Common.Infrastructure.Configurations
{
    /// <summary>
    /// Represents the <see cref="AuthorizationConfiguration"/> class.
    /// </summary>
    public sealed class AuthorizationConfiguration
    {
        /// <summary>
        /// Gets and sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets and sets the issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets and sets the grant type.
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// Gets and sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets and sets the jwks uri.
        /// </summary>
        public string JwksUri { get; set; }

        /// <summary>
        /// Gets and sets the token uri.
        /// </summary>
        public string TokenUri { get; set; }
    }
}
