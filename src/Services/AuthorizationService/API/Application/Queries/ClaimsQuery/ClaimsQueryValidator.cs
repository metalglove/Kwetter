using FluentValidation;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.ClaimsQuery
{
    /// <summary>
    /// Represents the <see cref="ClaimsQueryValidator"/> class.
    /// </summary>
    public sealed class ClaimsQueryValidator : AbstractValidator<ClaimsQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsQueryValidator"/> class.
        /// </summary>
        public ClaimsQueryValidator()
        {
            RuleFor(claimsQuery => claimsQuery.IdToken)
                .NotEmpty()
                .WithMessage("The id token cannot be empty.");
        }
    }
}
