using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.FollowService.API.Application.Commands.UpdateUserProfilePictureUrlCommand;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.API.Application.IntegrationEventHandlers.UserProfilePictureUrlUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfilePictureUrlUpdatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserProfilePictureUrlUpdatedIntegrationEventHandler : KwetterEventHandler<UserProfilePictureUrlUpdatedIntegrationEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePictureUrlUpdatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="serviceScopeFactory">The service scope factory.</param>
        public UserProfilePictureUrlUpdatedIntegrationEventHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Handles the user profile picture url updated integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user profile picture url updated integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserProfilePictureUrlUpdatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new UpdateUserProfilePictureUrlCommand()
            {
                UserId = @event.UserId,
                UserProfilePictureUrl = @event.UserProfilePictureUrl
            }, cancellationToken);
        }
    }
}
