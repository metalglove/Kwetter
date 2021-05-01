using System.Collections.Generic;

namespace Kwetter.Services.Common.Application.Dtos
{
    /// <summary>
    /// Represents the <see cref="ClaimsDto"/> class.
    /// </summary>
    public class ClaimsDto
    {
        /// <summary>
        /// Gets and sets the subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets and sets the issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets and sets the audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets and sets the expiration time in seconds.
        /// </summary>
        public long ExpirationTimeSeconds { get; set; }

        /// <summary>
        /// Gets and sets the issued at time in seconds.
        /// </summary>
        public long IssuedAtTimeSeconds { get; set; }

        /// <summary>
        /// Gets and sets the claims of the user.
        /// </summary>
        public IReadOnlyDictionary<string, ClaimDto> Claims { get; set; }
    }
}
