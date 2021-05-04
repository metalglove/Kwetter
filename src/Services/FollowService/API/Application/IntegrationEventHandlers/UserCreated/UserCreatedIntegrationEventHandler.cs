using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.FollowService.API.Application.Commands.CreateUserCommand;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.IntegrationEventHandlers.UserCreated
{
    /// <summary>
    /// Represents the <see cref="UserCreatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserCreatedIntegrationEventHandler : KwetterEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="serviceScopeFactory">The service scope factory.</param>
        public UserCreatedIntegrationEventHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Handles the user created integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user created integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new CreateUserCommand()
            {
                UserId = @event.UserId,
                UserDisplayName = @event.UserDisplayName,
                UserProfilePictureUrl = @event.UserProfilePictureUrl
            }, cancellationToken);
        }
    }
}
