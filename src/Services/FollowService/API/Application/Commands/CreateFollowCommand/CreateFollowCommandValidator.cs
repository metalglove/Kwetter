using FluentValidation;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand
{
    /// <summary>
    /// Represents the <see cref="CreateFollowCommandValidator"/> class.
    /// </summary>
    public sealed class CreateFollowCommandValidator : AbstractValidator<CreateFollowCommand>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFollowCommandValidator"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public CreateFollowCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            
            RuleFor(createFollowCommand => createFollowCommand)
                .CustomAsync(ValidateFollowAsync);
        }

        private async Task ValidateFollowAsync(CreateFollowCommand createFollowCommand, ValidationContext<CreateFollowCommand> context, CancellationToken cancellationToken)
        {
            if (createFollowCommand.FollowerId == Guid.Empty)
            {
                context.AddFailure("The follower id can not be empty.");
                return;
            }
            if (createFollowCommand.FollowingId == Guid.Empty)
            {
                context.AddFailure("The following id can not be empty.");
                return;
            }
            if (createFollowCommand.FollowingId == createFollowCommand.FollowerId)
            {
                context.AddFailure("The follow and following id are the same. One can not follow themself.");
                return;
            }
            UserAggregate userAggregate = await _userRepository.FindAsync(createFollowCommand.FollowerId, cancellationToken);
            if (userAggregate.Followings.Any(follow => follow.FollowingId == createFollowCommand.FollowingId)) 
                context.AddFailure("The follow already exists.");
        }
    }
}