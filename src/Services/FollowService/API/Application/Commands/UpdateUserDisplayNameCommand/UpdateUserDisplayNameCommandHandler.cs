using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.Commands.UpdateUserDisplayNameCommand
{
    /// <summary>
    /// Represents the <see cref="UpdateUserDisplayNameCommandHandler"/> class.
    /// </summary>
    public class UpdateUserDisplayNameCommandHandler : IRequestHandler<UpdateUserDisplayNameCommand, CommandResponse>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserDisplayNameCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UpdateUserDisplayNameCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="UpdateUserDisplayNameCommand"/>.
        /// </summary>
        /// <param name="request">The update user display name command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(UpdateUserDisplayNameCommand request, CancellationToken cancellationToken)
        {
            UserAggregate trackedUser = await _userRepository.FindAsync(request.UserId, cancellationToken);
            trackedUser?.UpdateUserDisplayName(request.UserDisplayName);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return new CommandResponse()
            {
                Success = trackedUser != default && success
            };
        }
    }
}
