using Kwetter.Services.Common.Application.CQRS;
using Kwetter.Services.Common.Application.Eventing.Integration;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Application.Eventing
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventHandler{TEventHandler, TEvent}"/> class.
    /// An eventhandler wrapper to handle integration events through the Mediator pipeline.
    /// </summary>
    /// <typeparam name="TEventHandler">The event handler type.</typeparam>
    /// <typeparam name="TEvent">The event type.</typeparam>
    public sealed class IntegrationEventHandler<TEventHandler, TEvent> : IRequestHandler<TEvent, CommandResponse>
        where TEventHandler : KwetterEventHandler<TEvent>
        where TEvent : IncomingIntegrationEvent
    {
        private readonly TEventHandler _eventHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventHandler{TEventHandler, TEvent}"/> class.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        public IntegrationEventHandler(TEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        /// <summary>
        /// Handles the integration event.
        /// </summary>
        /// <param name="request">The integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns a command response.</returns>
        public async Task<CommandResponse> Handle(TEvent request, CancellationToken cancellationToken)
        {
            await _eventHandler.HandleAsync(request, cancellationToken);
            return new CommandResponse() { Success = true };
        }
    }
}
