using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.FollowService.API.Application.Commands.UpdateUserDisplayNameCommand;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.IntegrationEventHandlers.UserDisplayNameUpdated
{
    /// <summary>
    /// Represents the <see cref="UserDisplayNameUpdatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserDisplayNameUpdatedIntegrationEventHandler : KwetterEventHandler<UserDisplayNameUpdatedIntegrationEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisplayNameUpdatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="serviceScopeFactory">The service scope factory.</param>
        public UserDisplayNameUpdatedIntegrationEventHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Handles the user display name updated integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user display name updated integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserDisplayNameUpdatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new UpdateUserDisplayNameCommand()
            {
                UserId = @event.UserId,
                UserDisplayName = @event.UserDisplayName
            }, cancellationToken);
        }
    }
}
