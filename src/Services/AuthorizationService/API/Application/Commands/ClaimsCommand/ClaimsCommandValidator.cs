using FluentValidation;
using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.API.Application.Commands.ClaimsCommand
{
    /// <summary>
    /// Represents the <see cref="ClaimsCommandValidator"/> class.
    /// </summary>
    public sealed class ClaimsCommandValidator : AbstractValidator<ClaimsCommand>
    {
        private readonly IIdentityRepository _identityRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsCommandValidator"/> class.
        /// </summary>
        /// <param name="identityRepository">The indentity repository.</param>
        public ClaimsCommandValidator(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
            
            RuleFor(claimsCommand => claimsCommand.IdToken)
                .NotEmpty()
                .WithMessage("The id token cannot be empty.");

            RuleFor(claimsCommand => claimsCommand.UserName)
                .CustomAsync(UserNameValidation);
        }

        private async Task UserNameValidation(string userName, ValidationContext<ClaimsCommand> context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                context.AddFailure("The user name cannot be empty.");
                return;
            }
            if (userName.Length > 32)
            {
                context.AddFailure("The user name length exceeded the maximum length of 32.");
                return;
            }
            if (!userName.All(char.IsLetterOrDigit))
            {
                context.AddFailure("The user name is not alphanumeric.");
                return;
            }
            IdentityAggregate identity = await _identityRepository.FindByUserNameAsync(userName, cancellationToken);
            if (identity != default)
            {
                context.AddFailure("The user name is not unique.");
                return;
            }
        }
    }
}
