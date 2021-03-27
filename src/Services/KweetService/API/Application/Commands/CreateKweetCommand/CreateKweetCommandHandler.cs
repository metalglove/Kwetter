using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Infrastructure.Integration;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly IIntegrationEventService _integrationEventService;
        private readonly ILogger<CreateKweetCommandHandler> _logger;
        private readonly IKweetRepository _kweetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateKweetCommandHandler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="kweetRepository">The kweet repository.</param>
        public CreateKweetCommandHandler(
            IIntegrationEventService integrationEventService,
            ILogger<CreateKweetCommandHandler> logger,
            IKweetRepository kweetRepository
            )
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _kweetRepository = kweetRepository ?? throw new ArgumentNullException(nameof(kweetRepository));
        }

        /// <summary>
        /// Handles the <see cref="CreateKweetCommand"/>.
        /// </summary>
        /// <param name="request">The create kweet command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public Task<CommandResponse> Handle(CreateKweetCommand request, CancellationToken cancellationToken)
        {
            KweetAggregate kweetAggregate = new(request.Id, request.UserId, request.Message);
            KweetAggregate trackedKweetAggregate = _kweetRepository.Create(kweetAggregate);
            CommandResponse commandResponse = new()
            {
                Success = trackedKweetAggregate != default
            };
            return Task.FromResult(commandResponse);
        }
    }
}