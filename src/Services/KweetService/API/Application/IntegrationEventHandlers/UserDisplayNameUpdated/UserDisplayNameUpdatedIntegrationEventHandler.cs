using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.API.Application.IntegrationEventHandlers.UserDisplayNameUpdated
{
    /// <summary>
    /// Represents the <see cref="UserDisplayNameUpdatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserDisplayNameUpdatedIntegrationEventHandler : KwetterEventHandler<UserDisplayNameUpdatedIntegrationEvent>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisplayNameUpdatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserDisplayNameUpdatedIntegrationEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Handles the user display name updated integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user display name updated integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserDisplayNameUpdatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            UserAggregate trackedUser = await _userRepository.FindAsync(@event.UserId, cancellationToken);
            trackedUser?.UpdateUserDisplayName(@event.UserDisplayName);
            bool success = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (!success)
                throw new KweetIntegrationException("Failed to handle UserDisplayNameUpdatedIntegrationEvent.");
        }
    }
}
