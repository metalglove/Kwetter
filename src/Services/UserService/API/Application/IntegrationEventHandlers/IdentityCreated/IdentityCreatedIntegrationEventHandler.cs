using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
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
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityCreatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public IdentityCreatedIntegrationEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the identity created integration event from the authorization service asynchronously.
        /// </summary>
        /// <param name="event">The identity created integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(IdentityCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            UserAggregate user = await _userRepository.FindByIdAsync(@event.UserId, cancellationToken);
            if (user != default)
                throw new UserIntegrationException("A user with the proposed user id already exists.");
            user = await _userRepository.FindByUserNameAsync(@event.UserName, cancellationToken);
            if (user != default)
                throw new UserIntegrationException("A user with the proposed user name already exists.");
            user = new(@event.UserId, @event.GivenName, @event.UserName, $"Hello! I am {@event.GivenName}!", @event.ProfilePictureUrl);
            _userRepository.Create(user);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (!success)
                throw new UserIntegrationException("Failed to handle IdentityCreatedIntegrationEvent.");
        }
    }
}
