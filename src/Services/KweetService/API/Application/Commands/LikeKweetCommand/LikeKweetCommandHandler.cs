using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.Commands.LikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="LikeKweetCommand"/> class.
    /// </summary>
    public sealed class LikeKweetCommandHandler : IRequestHandler<LikeKweetCommand, CommandResponse>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeKweetCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public LikeKweetCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="LikeKweetCommand"/>.
        /// </summary>
        /// <param name="request">The like kweet command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(LikeKweetCommand request, CancellationToken cancellationToken)
        {
            UserAggregate user = await _userRepository.FindAsync(request.UserId, cancellationToken);
            Kweet kweet = await _userRepository.FindKweetAsync(request.KweetId, cancellationToken);
            bool liked = user.LikeKweet(kweet);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            CommandResponse commandResponse = new()
            {
                Success = liked && success,
                Errors = liked 
                    ? new List<string>() 
                    : new List<string>() { "The kweet is already liked." }
            };
            return commandResponse;
        }
    }
}