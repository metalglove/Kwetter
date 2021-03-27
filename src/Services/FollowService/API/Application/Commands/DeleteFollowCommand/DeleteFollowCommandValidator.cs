using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;

namespace Kwetter.Services.FollowService.API.Application.Commands.DeleteFollowCommand
{
    /// <summary>
    /// Represents the <see cref="DeleteFollowCommandValidator"/> class.
    /// </summary>
    public sealed class DeleteFollowCommandValidator : AbstractValidator<DeleteFollowCommand>
    {
        private readonly IFollowRepository _followRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFollowCommandValidator"/> class.
        /// </summary>
        /// <param name="followRepository">The follow repository.</param>
        public DeleteFollowCommandValidator(IFollowRepository followRepository)
        {
            _followRepository = followRepository ?? throw new ArgumentNullException(nameof(followRepository));
            
            RuleFor(deleteFollowCommand => deleteFollowCommand)
                .CustomAsync(ValidateUnfollowAsync);
        }

        private async Task ValidateUnfollowAsync(DeleteFollowCommand deleteFollowCommand, CustomContext context, CancellationToken cancellationToken)
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
            FollowAggregate follow = await _followRepository.FindAsync(deleteFollowCommand.FollowingId, deleteFollowCommand.FollowerId, cancellationToken);
            if (follow == null) 
                context.AddFailure("The follow does not exist.");
        }
    }
}