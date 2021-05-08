using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserProfilePictureUrlUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfilePictureUrlUpdatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserProfilePictureUrlUpdatedIntegrationEventHandler : KwetterEventHandler<UserProfilePictureUrlUpdatedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePictureUrlUpdatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public UserProfilePictureUrlUpdatedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the user profile picture url updated integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user profile picture url updated integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserProfilePictureUrlUpdatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            bool success = await _timelineGraphStore.UpdateUserProfilePictureUrlAsync(@event.UserId, @event.UserProfilePictureUrl);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle UserProfilePictureUrlUpdatedIntegrationEvent.");
        }
    }
}
