using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.IntegrationEventHandlers.UserCreated
{
    /// <summary>
    /// Represents the <see cref="UserCreatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserCreatedIntegrationEventHandler : KwetterEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserCreatedIntegrationEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the user created integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user created integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            UserAggregate user = new(@event.UserId, @event.UserDisplayName, @event.UserName, @event.UserProfilePictureUrl);
            _userRepository.Create(user);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (!success)
                throw new KweetIntegrationException("Failed to handle UserCreatedIntegrationEvent.");
        }
    }
}
