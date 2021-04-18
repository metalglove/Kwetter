using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers
{
    public class UserCreatedDomainEventListener : KwetterEventHandler<UserCreatedDomainEvent>
    {
        private readonly ILogger<UserCreatedDomainEventListener> _logger;
        private readonly IEventSerializer _eventSerializer;

        public UserCreatedDomainEventListener(ILogger<UserCreatedDomainEventListener> logger, IEventSerializer eventSerializer)
        {
            _logger = logger;
            _eventSerializer = eventSerializer;
        }
        public override ValueTask HandleAsync(UserCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_eventSerializer.SerializeToString(@event));
            return ValueTask.CompletedTask;
        }
    }
}
