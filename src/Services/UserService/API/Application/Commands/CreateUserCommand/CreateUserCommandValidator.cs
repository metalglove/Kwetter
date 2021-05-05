using FluentValidation;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.Commands.CreateUserCommand
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommandValidator"/> class.
    /// </summary>
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandValidator"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public CreateUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            
            RuleFor(createUserCommand => createUserCommand.UserDisplayName)
                .Custom(ValidateUserDisplayName);

            RuleFor(createUserCommand => createUserCommand.UserName)
                .Custom(ValidateUserName);

            RuleFor(createUserCommand => createUserCommand.UserId)
                .CustomAsync(CheckUserIdUniquenessAsync);

            RuleFor(createUserCommand => createUserCommand.UserProfileDescription)
                .NotNull()
                .WithMessage("The profile description is null.");

            RuleFor(createUserCommand => createUserCommand.UserProfilePictureUrl)
                .NotEmpty()
                .WithMessage("The profile picture url is null, empty or contains only white spaces.");
        }

        private void ValidateUserDisplayName(string displayName, ValidationContext<CreateUserCommand> context)
        {
            // Checks whether the display name is not null, empty or contains only white spaces.
            if (string.IsNullOrWhiteSpace(displayName))
            {
                context.AddFailure("The display name must not be null or empty.");
                return;
            }

            // Checks whether the display name does not exceed 64 characters.
            if (displayName.Length > 64)
            {
                context.AddFailure("The display name must not exceed 64 characters.");
                return;
            }
        }

        private void ValidateUserName(string userName, ValidationContext<CreateUserCommand> context)
        {
            // Checks whether the user name is not null, empty or contains only white spaces.
            if (string.IsNullOrWhiteSpace(userName))
            {
                context.AddFailure("The user name must not be null, empty or only contain whitespaces.");
                return;
            }

            // Checks whether the user name does not exceed 64 characters.
            if (userName.Length > 32)
            {
                context.AddFailure("The user name must not exceed 32 characters.");
                return;
            }

            // Checks whether the user name is alphanumeric.
            if (!userName.All(char.IsLetterOrDigit))
            {
                context.AddFailure("The user name must be alphanumeric.");
                return;
            }
        }

        private async Task CheckUserIdUniquenessAsync(Guid proposedUserId, ValidationContext<CreateUserCommand> context, CancellationToken cancellationToken)
        {
            // Checks whether the user id is empty.
            if (proposedUserId == Guid.Empty)
            {
                context.AddFailure("The user id can not be empty.");
                return;
            }
            // Checks whether the user id already exists.
            UserAggregate userAggregate = await _userRepository.FindByIdAsync(proposedUserId, cancellationToken);
            if (userAggregate != default)
            {
                context.AddFailure("A user with the proposed user id already exists.");
                return;
            }
        }
    }
}
