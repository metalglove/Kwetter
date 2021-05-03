using FluentValidation;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.Commands.DeleteFollowCommand
{
    /// <summary>
    /// Represents the <see cref="DeleteFollowCommandValidator"/> class.
    /// </summary>
    public sealed class DeleteFollowCommandValidator : AbstractValidator<DeleteFollowCommand>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFollowCommandValidator"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public DeleteFollowCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            
            RuleFor(deleteFollowCommand => deleteFollowCommand)
                .CustomAsync(ValidateUnfollowAsync);
        }

        private async Task ValidateUnfollowAsync(DeleteFollowCommand deleteFollowCommand, ValidationContext<DeleteFollowCommand> context, CancellationToken cancellationToken)
        {
            if (deleteFollowCommand.FollowerId == Guid.Empty)
            {
                context.AddFailure("The follower id can not be empty.");
                return;
            }
            if (deleteFollowCommand.FollowingId == Guid.Empty)
            {
                context.AddFailure("The following id can not be empty.");
                return;
            }
            UserAggregate userAggregate = await _userRepository.FindAsync(deleteFollowCommand.FollowerId, cancellationToken);
            bool hasFollow = userAggregate.Followings.Any(follow => follow.FollowingId == deleteFollowCommand.FollowingId);
            if (!hasFollow)
                context.AddFailure("The follow does not exist.");
        }
    }
}