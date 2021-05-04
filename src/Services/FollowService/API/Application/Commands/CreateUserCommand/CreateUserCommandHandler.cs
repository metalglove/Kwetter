using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.Commands.CreateUserCommand
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommandHandler"/> class.
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommandResponse>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="CreateUserCommand"/>.
        /// </summary>
        /// <param name="request">The create user command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            UserAggregate user = new(request.UserId, request.UserDisplayName, request.UserProfilePictureUrl);
            UserAggregate trackedUser = _userRepository.Create(user);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return new CommandResponse()
            {
                Success = trackedUser != default && success
            };
        }
    }
}
