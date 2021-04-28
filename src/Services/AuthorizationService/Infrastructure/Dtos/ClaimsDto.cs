using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kwetter.Services.AuthorizationService.Infrastructure.Dtos
{
    /// <summary>
    /// Represents the <see cref="ClaimsDto"/> class.
    /// </summary>
    public class ClaimsDto
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the claims of the user.
        /// </summary>
        [JsonPropertyName("claims")]
        public List<ClaimDto> Claims { get; set; }

        /// <summary>
        /// Gets and sets the id token.
        /// </summary>
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
    }

    /// <summary>
    /// Represents the <see cref="ClaimDto"/> class.
    /// </summary>
    public class ClaimDto
    {
        /// <summary>
        /// Gets and sets the claim type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets and sets the claim value.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
