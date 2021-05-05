using FluentValidation;
using System.Linq;

namespace Kwetter.Services.AuthorizationService.API.Application.Queries.VerifyUserNameUniquenessQuery
{
    /// <summary>
    /// Represents the <see cref="VerifyUserNameUniquenessQueryValidator"/> class.
    /// </summary>
    public sealed class VerifyUserNameUniquenessQueryValidator : AbstractValidator<VerifyUserNameUniquenessQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerifyUserNameUniquenessQueryValidator"/> class.
        /// </summary>
        public VerifyUserNameUniquenessQueryValidator()
        {
            RuleFor(verifyUserNameUniquenessQuery => verifyUserNameUniquenessQuery.UserName)
                .Custom(UserNameValidation);
        }

        private void UserNameValidation(string userName, ValidationContext<VerifyUserNameUniquenessQuery> context)
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
        }
    }
}
