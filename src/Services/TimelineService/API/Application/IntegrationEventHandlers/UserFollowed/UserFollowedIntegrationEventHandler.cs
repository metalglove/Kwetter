using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Domain;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserFollowed
{
    /// <summary>
    /// Represents the <see cref="UserFollowedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserFollowedIntegrationEventHandler : KwetterEventHandler<UserFollowedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFollowedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public UserFollowedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the user followed integration event from the follow service asynchronously.
        /// </summary>
        /// <param name="event">The user followed integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserFollowedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            Follow follow = new()
            {
                FollowerId = @event.FollowerId,
                FollowingId = @event.FollowingId,
                FollowDateTime = @event.FollowedDateTime
            };
            bool success = await _timelineGraphStore.CreateFollowerAsync(follow);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle UserFollowedIntegrationEvent.");
        }
    }
}
