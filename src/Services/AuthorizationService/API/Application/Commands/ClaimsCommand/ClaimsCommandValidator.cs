using FluentValidation;

namespace Kwetter.Services.AuthorizationService.API.Application.Commands.ClaimsCommand
{
    /// <summary>
    /// Represents the <see cref="ClaimsCommandValidator"/> class.
    /// </summary>
    public sealed class ClaimsCommandValidator : AbstractValidator<ClaimsCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsCommandValidator"/> class.
        /// </summary>
        public ClaimsCommandValidator()
        {
            RuleFor(claimsCommand => claimsCommand.IdToken)
                .NotEmpty()
                .WithMessage("The id token cannot be empty.");
        }
    }
}
