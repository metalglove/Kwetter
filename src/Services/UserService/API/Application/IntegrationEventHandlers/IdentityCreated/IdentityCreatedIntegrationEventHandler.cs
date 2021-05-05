using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.UserService.API.Application.Commands.CreateUserCommand;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.API.Application.IntegrationEventHandlers.IdentityCreated
{
    /// <summary>
    /// Represents the <see cref="IdentityCreatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class IdentityCreatedIntegrationEventHandler : KwetterEventHandler<IdentityCreatedIntegrationEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityCreatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="serviceScopeFactory">The service scope factory.</param>
        public IdentityCreatedIntegrationEventHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        /// <summary>
        /// Handles the identity created integration event from the authorization service asynchronously.
        /// </summary>
        /// <param name="event">The identity created integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(IdentityCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new CreateUserCommand()
            {
                UserId = @event.UserId,
                UserDisplayName = @event.GivenName,
                UserName = @event.UserName,
                UserProfileDescription = $"Hello! I am {@event.GivenName}!",
                UserProfilePictureUrl = @event.ProfilePictureUrl
            }, cancellationToken);
        }
    }
}
