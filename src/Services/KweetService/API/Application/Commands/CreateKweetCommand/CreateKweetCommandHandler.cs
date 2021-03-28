using Kwetter.Services.Common.API.CQRS;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;

namespace Kwetter.Services.KweetService.API.Application.Commands.CreateKweetCommand
{
    /// <summary>
    /// Represents the <see cref="CreateKweetCommand"/> class.
    /// </summary>
    public sealed class CreateKweetCommandHandler : IRequestHandler<CreateKweetCommand, CommandResponse>
    {
        private readonly IKweetRepository _kweetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateKweetCommandHandler"/> class.
        /// </summary>
        /// <param name="kweetRepository">The kweet repository.</param>
        public CreateKweetCommandHandler(IKweetRepository kweetRepository)
        {
            _kweetRepository = kweetRepository ?? throw new ArgumentNullException(nameof(kweetRepository));
        }

        /// <summary>
        /// Handles the <see cref="CreateKweetCommand"/>.
        /// </summary>
        /// <param name="request">The create kweet command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(CreateKweetCommand request, CancellationToken cancellationToken)
        {
            KweetAggregate kweetAggregate = new(request.KweetId, request.UserId, request.Message);
            KweetAggregate trackedKweetAggregate = _kweetRepository.Create(kweetAggregate);
            bool success = await _kweetRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            CommandResponse commandResponse = new()
            {
                Success = trackedKweetAggregate != default && success
            };
            return commandResponse;
        }
    }
}