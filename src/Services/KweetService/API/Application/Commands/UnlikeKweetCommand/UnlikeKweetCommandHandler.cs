using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.Commands.UnlikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="UnlikeKweetCommand"/> class.
    /// </summary>
    public sealed class UnlikeKweetCommandHandler : IRequestHandler<UnlikeKweetCommand, CommandResponse>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlikeKweetCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UnlikeKweetCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="UnlikeKweetCommand"/>.
        /// </summary>
        /// <param name="request">The unlike kweet command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(UnlikeKweetCommand request, CancellationToken cancellationToken)
        {
            UserAggregate user = await _userRepository.FindAsync(request.UserId, cancellationToken);
            Kweet kweet = await _userRepository.FindKweetAsync(request.KweetId, cancellationToken);
            bool unliked = user.UnlikeKweet(kweet);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            CommandResponse commandResponse = new()
            {
                Success = unliked && success,
                Errors = unliked 
                    ? new List<string>() 
                    : new List<string>() { "The kweet was not liked." }
            };
            return commandResponse;
        }
    }
}