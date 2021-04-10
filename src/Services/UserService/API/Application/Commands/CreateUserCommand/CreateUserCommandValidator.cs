using FluentValidation;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using System;
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
                .CustomAsync(ValidateUserDisplayNameAsync);

            RuleFor(createUserCommand => createUserCommand.UserId)
                .CustomAsync(CheckUserIdUniqueness);
        }

        private async Task ValidateUserDisplayNameAsync(string displayName, ValidationContext<CreateUserCommand> context, CancellationToken cancellationToken)
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

            // Checks whether the display name already exists.
            UserAggregate userAggregate = await _userRepository.FindByUserDisplayNameAsync(displayName, cancellationToken);
            if (userAggregate != default)
            {
                context.AddFailure("The display name already exists.");
                return;
            }
        }

        private async Task CheckUserIdUniqueness(Guid proposedUserId, ValidationContext<CreateUserCommand> context, CancellationToken cancellationToken)
        {
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
