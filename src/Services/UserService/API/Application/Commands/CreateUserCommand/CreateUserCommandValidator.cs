using FluentValidation;
using FluentValidation.Validators;
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
            
            // Applies one custom username validation rule.
            RuleFor(createUserCommand => createUserCommand.Username)
                .CustomAsync(ValidateUsernameAsync);

            // Applies one custom username validation rule.
            RuleFor(createUserCommand => createUserCommand.UserId)
                .CustomAsync(CheckUserIdUniqueness);
        }

        private async Task ValidateUsernameAsync(string username, CustomContext context, CancellationToken cancellationToken)
        {
            // Checks whether the username is not null, empty or contains only white spaces.
            if (string.IsNullOrWhiteSpace(username))
            {
                context.AddFailure("The username must not be null or empty.");
                return;
            }

            // Checks whether the username does not exceed 64 characters.
            if (username.Length > 64)
            {
                context.AddFailure("The username must not exceed 64 characters.");
                return;
            }

            // Checks whether the username already exists.
            UserAggregate userAggregate = await _userRepository.FindByUsernameAsync(username, cancellationToken);
            if (userAggregate != default)
            {
                context.AddFailure("The username already exists.");
                return;
            }
        }

        private async Task CheckUserIdUniqueness(Guid proposedUserId, CustomContext context, CancellationToken cancellationToken)
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
