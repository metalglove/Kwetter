using FluentValidation;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.Commands.LikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="LikeKweetCommandValidator"/> class.
    /// </summary>
    public sealed class LikeKweetCommandValidator : AbstractValidator<LikeKweetCommand>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeKweetCommandValidator"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public LikeKweetCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
            RuleFor(likeKweetCommand => likeKweetCommand)
                .CustomAsync(LikeKweetValidationAsync);
        }

        private async Task LikeKweetValidationAsync(LikeKweetCommand likeKweetCommand, ValidationContext<LikeKweetCommand> context, CancellationToken cancellationToken)
        {
            if (likeKweetCommand.KweetId == Guid.Empty)
            {
                context.AddFailure("The kweet id can not be empty.");
                return;
            }
            if (likeKweetCommand.UserId == Guid.Empty)
            {
                context.AddFailure("The user id can not be empty.");
                return;
            }
            Kweet kweet = await _userRepository.FindKweetAsync(likeKweetCommand.KweetId, cancellationToken);
            if (kweet == default) 
                context.AddFailure("The kweet does not exist.");
        }
    }
}