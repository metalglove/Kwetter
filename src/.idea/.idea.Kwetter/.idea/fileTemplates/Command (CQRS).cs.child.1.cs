using Kwetter.Services.Common.API.CQRS;
using Kwetter.Services.Common.Infrastructure.Integration;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ${NAMESPACE}
{
    /// <summary>
    /// Represents the <see cref="${NAME}"/> class.
    /// </summary>
    public sealed class ${NAME}Handler : IRequestHandler<${NAME}, CommandResponse>
    {
        private readonly IIntegrationEventService _integrationEventService;
        private readonly ILogger<${NAME}Handler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="${NAME}Handler"/> class.
        /// </summary>
        /// <param name="integrationEventService">The integration event service.</param>
        /// <param name="logger">The logger.</param>
        public ${NAME}Handler(
            IIntegrationEventService integrationEventService,
            ILogger<${NAME}Handler> logger
            )
        {
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handles the <see cref="${NAME}"/>.
        /// </summary>
        /// <param name="request">The ... command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the command response.</returns>
        public async Task<CommandResponse> Handle(${NAME} request, CancellationToken cancellationToken)
        {
            CommandResponse commandResponse = new()
            {
                Success = true
            };

            return commandResponse;
        }
    }
}