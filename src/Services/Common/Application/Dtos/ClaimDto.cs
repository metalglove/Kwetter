using System.Security.Claims;

namespace Kwetter.Services.Common.Application.Dtos
{
    /// <summary>
    /// Represents the <see cref="ClaimDto"/> class.
    /// </summary>
    public class ClaimDto
    {
        /// <summary>
        /// Gets and sets the claim name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the claim value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Converts the claim dto to a claim.
        /// </summary>
        /// <returns></returns>
        public Claim ToClaim() => new(Name, Value);
    }
}
