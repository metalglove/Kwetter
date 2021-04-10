using FluentValidation;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.AuthorizationQuery
{
    /// <summary>
    /// Represents the <see cref="AuthorizationQueryValidator"/> class.
    /// </summary>
    public sealed class AuthorizationQueryValidator : AbstractValidator<AuthorizationQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationQueryValidator"/> class.
        /// </summary>
        public AuthorizationQueryValidator()
        {
            RuleFor(authorizationQuery => authorizationQuery.Code)
                .NotEmpty()
                .WithMessage("The code cannot be empty.");
        }
    }
}
