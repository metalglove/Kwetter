using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;

namespace Kwetter.Services.KweetService.API.Application.Commands.UnlikeKweetCommand
{
    /// <summary>
    /// Represents the <see cref="UnlikeKweetCommand"/> class.
    /// </summary>
    public sealed class UnlikeKweetCommandHandler : IRequestHandler<UnlikeKweetCommand, CommandResponse>
    {
        private readonly IKweetRepository _kweetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlikeKweetCommandHandler"/> class.
        /// </summary>
        /// <param name="kweetRepository">The kweet repository.</param>
        public UnlikeKweetCommandHandler(IKweetRepository kweetRepository)
        {
            _kweetRepository = kweetRepository ?? throw new ArgumentNullException(nameof(kweetRepository));
        }

        /// <summary>
        /// Handles the <see cref="UnlikeKweetCommand"/>.
        /// </summary>
        /// <param name="request">The unlike kweet command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(UnlikeKweetCommand request, CancellationToken cancellationToken)
        {
            KweetAggregate kweetAggregate = await _kweetRepository.FindAsync(request.KweetId, cancellationToken);
            bool unliked = kweetAggregate.RemoveLike(request.UserId);
            bool success = await _kweetRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
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