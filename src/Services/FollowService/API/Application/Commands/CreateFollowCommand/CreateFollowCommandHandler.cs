using System;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand
{
    /// <summary>
    /// Represents the <see cref="CreateFollowCommand"/> class.
    /// </summary>
    public sealed class CreateFollowCommandHandler : IRequestHandler<CreateFollowCommand, CommandResponse>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFollowCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public CreateFollowCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="CreateFollowCommand"/>.
        /// </summary>
        /// <param name="request">The create follow command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(CreateFollowCommand request, CancellationToken cancellationToken)
        {
            UserAggregate userAggregate = await _userRepository.FindAsync(request.FollowerId, cancellationToken);
            UserAggregate otherUserAggregate = await _userRepository.FindAsync(request.FollowingId, cancellationToken);
            bool followed = userAggregate.Follow(otherUserAggregate);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return new CommandResponse()
            {
                Success = followed && success
            };
        }
    }
}