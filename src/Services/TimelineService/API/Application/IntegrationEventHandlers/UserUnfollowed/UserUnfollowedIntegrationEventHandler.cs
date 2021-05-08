using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserUnfollowed
{
    /// <summary>
    /// Represents the <see cref="UserUnfollowedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserUnfollowedIntegrationEventHandler : KwetterEventHandler<UserUnfollowedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUnfollowedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public UserUnfollowedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the user unfollowed integration event from the follow service asynchronously.
        /// </summary>
        /// <param name="event">The user unfollowed integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserUnfollowedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            bool success = await _timelineGraphStore.DeleteFollowerAsync(@event.FollowerId, @event.FollowingId);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle UserUnfollowedIntegrationEvent.");
        }
    }
}
