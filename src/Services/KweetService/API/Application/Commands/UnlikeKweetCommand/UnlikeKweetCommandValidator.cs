using FluentValidation;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.Commands.UnlikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="UnlikeKweetCommandValidator"/> class.
    /// </summary>
    public sealed class UnlikeKweetCommandValidator : AbstractValidator<UnlikeKweetCommand>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlikeKweetCommandValidator"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UnlikeKweetCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            RuleFor(unlikeKweetCommand => unlikeKweetCommand)
                .CustomAsync(UnlikeKweetValidationAsync);
        }

        private async Task UnlikeKweetValidationAsync(UnlikeKweetCommand unlikeKweetCommand, ValidationContext<UnlikeKweetCommand> context, CancellationToken cancellationToken)
        {
            if (unlikeKweetCommand.KweetId == Guid.Empty)
            {
                context.AddFailure("The kweet id can not be empty.");
                return;
            }
            if (unlikeKweetCommand.UserId == Guid.Empty)
            {
                context.AddFailure("The user id can not be empty.");
                return;
            }
            Kweet kweet = await _userRepository.FindKweetAsync(unlikeKweetCommand.KweetId, cancellationToken);
            if (kweet == default) 
                context.AddFailure("The kweet does not exist.");
        }
    }
}