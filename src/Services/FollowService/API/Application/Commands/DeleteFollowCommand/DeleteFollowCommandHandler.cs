using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.Commands.DeleteFollowCommand
{
    /// <summary>
    /// Represents the <see cref="DeleteFollowCommand"/> class.
    /// </summary>
    public sealed class DeleteFollowCommandHandler : IRequestHandler<DeleteFollowCommand, CommandResponse>
    {
        private readonly IUserRepository _userRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFollowCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public DeleteFollowCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="DeleteFollowCommand"/>.
        /// </summary>
        /// <param name="request">The delete follow command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(DeleteFollowCommand request, CancellationToken cancellationToken)
        {
            UserAggregate userAggregate = await _userRepository.FindAsync(request.FollowerId, cancellationToken);
            UserAggregate otherUserAggregate = await _userRepository.FindAsync(request.FollowingId, cancellationToken);
            bool unfollowed = userAggregate.Unfollow(otherUserAggregate);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return new CommandResponse()
            {
                Success = unfollowed && success
            };
        }
    }
}