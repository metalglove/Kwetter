using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.Commands.UpdateUserProfilePictureUrlCommand
{
    /// <summary>
    /// Represents the <see cref="UpdateUserProfilePictureUrlCommandHandler"/> class.
    /// </summary>
    public class UpdateUserProfilePictureUrlCommandHandler : IRequestHandler<UpdateUserProfilePictureUrlCommand, CommandResponse>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserProfilePictureUrlCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UpdateUserProfilePictureUrlCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="UpdateUserProfilePictureUrlCommand"/>.
        /// </summary>
        /// <param name="request">The update user profile picture url command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(UpdateUserProfilePictureUrlCommand request, CancellationToken cancellationToken)
        {
            UserAggregate trackedUser = await _userRepository.FindAsync(request.UserId, cancellationToken);
            trackedUser?.UpdateUserProfilePictureUrl(request.UserProfilePictureUrl);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return new CommandResponse()
            {
                Success = trackedUser != default && success
            };
        }
    }
}
