using System;
using System.Text.Json.Serialization;

namespace Kwetter.Services.AuthorizationService.Infrastructure.Dtos
{
    /// <summary>
    /// Represents the <see cref="AuthorizationDto"/> record.
    /// </summary>
    public record AuthorizationDto
    {
        /// <summary>
        /// Gets and sets the access token.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets and sets the seconds the access token expires in.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets and sets the scope of the authorization.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Gets and sets the refresh token to refresh the access token.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets and sets the id token.
        /// </summary>
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

        /// <summary>
        /// Gets and sets the token type.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
    }
}
