using Kwetter.Services.Common.EventBus.Abstractions;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers
{
    public class UserCreatedDomainEventListener : KwetterEventHandler<UserCreatedDomainEvent>
    {
        private readonly ILogger<UserCreatedDomainEventListener> _logger;
        private readonly IMessageSerializer _messageSerializer;

        public UserCreatedDomainEventListener(ILogger<UserCreatedDomainEventListener> logger, IMessageSerializer messageSerializer)
        {
            _logger = logger;
            _messageSerializer = messageSerializer;
        }
        public override ValueTask HandleAsync(UserCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_messageSerializer.SerializeToString(@event));
            return ValueTask.CompletedTask;
        }
    }
}
