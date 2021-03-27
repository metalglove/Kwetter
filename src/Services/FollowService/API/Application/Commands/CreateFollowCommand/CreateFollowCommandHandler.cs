using System;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;
using MediatR;

namespace Kwetter.Services.FollowService.API.Application.Commands.CreateFollowCommand
{
    /// <summary>
    /// Represents the <see cref="CreateFollowCommand"/> class.
    /// </summary>
    public sealed class CreateFollowCommandHandler : IRequestHandler<CreateFollowCommand, CommandResponse>
    {
        private readonly IFollowRepository _followRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFollowCommandHandler"/> class.
        /// </summary>
        /// <param name="followRepository">The follow repository.</param>
        public CreateFollowCommandHandler(IFollowRepository followRepository)
        {
            _followRepository = followRepository ?? throw new ArgumentNullException(nameof(followRepository));
        }

        /// <summary>
        /// Handles the <see cref="CreateFollowCommand"/>.
        /// </summary>
        /// <param name="request">The create follow command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(CreateFollowCommand request, CancellationToken cancellationToken)
        {
            FollowAggregate followAggregate = new(request.FollowingId, request.FollowerId);
            FollowAggregate trackedFollowerAggregate = _followRepository.Create(followAggregate);
            bool success = await _followRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            CommandResponse commandResponse = new()
            {
                Success = trackedFollowerAggregate != default && success
            };
            return commandResponse;
        }
    }
}