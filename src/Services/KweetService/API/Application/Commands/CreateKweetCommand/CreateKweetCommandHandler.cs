using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand
{
    /// <summary>
    /// Represents the <see cref="CreateKweetCommand"/> class.
    /// </summary>
    public sealed class CreateKweetCommandHandler : IRequestHandler<CreateKweetCommand, CommandResponse>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateKweetCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public CreateKweetCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the <see cref="CreateKweetCommand"/>.
        /// </summary>
        /// <param name="request">The create kweet command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(CreateKweetCommand request, CancellationToken cancellationToken)
        {
            UserAggregate user = await _userRepository.FindAsync(request.UserId, cancellationToken);
            Task<IEnumerable<Mention>> FindUsersByUserNamesAsync(IEnumerable<Mention> mentions, CancellationToken ct)
                => _userRepository.FindUsersByUserNameAndTrackMentionsAsync(mentions, ct);
            Kweet kweet = await user.CreateKweetAsync(request.KweetId, request.Message, FindUsersByUserNamesAsync, cancellationToken);
            kweet = _userRepository.TrackKweet(kweet);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            CommandResponse commandResponse = new()
            {
                Success = kweet != default && success
            };
            return commandResponse;
        }
    }
}