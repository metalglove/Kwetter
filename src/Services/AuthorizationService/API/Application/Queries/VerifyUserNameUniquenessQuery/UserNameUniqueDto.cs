namespace Kwetter.Services.AuthorizationService.API.Application.Queries.VerifyUserNameUniquenessQuery
{
    /// <summary>
    /// Represents the <see cref="UserNameUniqueDto"/> record.
    /// </summary>
    public record UserNameUniqueDto
    {
        /// <summary>
        /// Gets and sets a value indicating whether the user name is unique.
        /// </summary>
        public bool IsUnique { get; set; }
    }
}
