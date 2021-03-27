using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;

namespace Kwetter.Services.FollowService.API.Application.Commands.DeleteFollowCommand
{
    /// <summary>
    /// Represents the <see cref="DeleteFollowCommand"/> class.
    /// </summary>
    public sealed class DeleteFollowCommandHandler : IRequestHandler<DeleteFollowCommand, CommandResponse>
    {
        private readonly IFollowRepository _followRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFollowCommandHandler"/> class.
        /// </summary>
        /// <param name="followRepository">The follow repository.</param>
        public DeleteFollowCommandHandler(IFollowRepository followRepository)
        {
            _followRepository = followRepository ?? throw new ArgumentNullException(nameof(followRepository));
        }

        /// <summary>
        /// Handles the <see cref="DeleteFollowCommand"/>.
        /// </summary>
        /// <param name="request">The delete follow command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(DeleteFollowCommand request, CancellationToken cancellationToken)
        {
            FollowAggregate followAggregate = await _followRepository.FindAsync(request.FollowingId, request.FollowerId, cancellationToken);
            bool unfollowed = followAggregate.Unfollow();
            _followRepository.Delete(followAggregate);
            bool success = await _followRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            CommandResponse commandResponse = new()
            {
                Success = unfollowed && success
            };
            return commandResponse;
        }
    }
}