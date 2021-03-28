using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;

namespace Kwetter.Services.KweetService.API.Application.Commands.LikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="LikeKweetCommand"/> class.
    /// </summary>
    public sealed class LikeKweetCommandHandler : IRequestHandler<LikeKweetCommand, CommandResponse>
    {
        private readonly IKweetRepository _kweetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeKweetCommandHandler"/> class.
        /// </summary>
        /// <param name="kweetRepository">The kweet repository.</param>
        public LikeKweetCommandHandler(IKweetRepository kweetRepository)
        {
            _kweetRepository = kweetRepository ?? throw new ArgumentNullException(nameof(kweetRepository));
        }

        /// <summary>
        /// Handles the <see cref="LikeKweetCommand"/>.
        /// </summary>
        /// <param name="request">The like kweet command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(LikeKweetCommand request, CancellationToken cancellationToken)
        {
            KweetAggregate kweetAggregate = await _kweetRepository.FindAsync(request.KweetId, cancellationToken);
            bool liked = kweetAggregate.AddLike(request.UserId);
            bool success = await _kweetRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
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