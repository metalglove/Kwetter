using Kwetter.Services.Common.Application.CQRS;
using MediatR;

namespace Kwetter.Services.Common.Application.Eventing.Integration
{
    /// <summary>
    /// Represents the <see cref="IncomingIntegrationEvent"/> class.
    /// Used as marker class for integration events to enable the event to go through the validation, exception, and transaction pipeline.
    /// </summary>
    public abstract class IncomingIntegrationEvent : IntegrationEvent, IRequest<CommandResponse>
    {

    }
}
