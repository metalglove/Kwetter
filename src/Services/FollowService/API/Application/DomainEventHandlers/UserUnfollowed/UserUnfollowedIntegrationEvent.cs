using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.FollowService.API.Application.DomainEventHandlers.UserUnfollowed
{
    /// <summary>
    /// Represents the <see cref="UserUnfollowedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserUnfollowedIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Gets and sets the following id.
        /// </summary>
        public Guid FollowingId { get; private set; }

        /// <summary>
        /// Gets and sets the follower id.
        /// </summary>
        public Guid FollowerId { get; private set; }

        /// <summary>
        /// Gets and sets the date time when the following was unfollowed.
        /// </summary>
        public DateTime UnfollowedDateTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUnfollowedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="followingId">The following id.</param>
        /// <param name="followerId">The follower id.</param>
        /// <param name="unfollowedDateTime">The unfollowed date time.</param>
        public UserUnfollowedIntegrationEvent(Guid followingId, Guid followerId, DateTime unfollowedDateTime)
        {
            FollowingId = followingId;
            FollowerId = followerId;
            UnfollowedDateTime = unfollowedDateTime;
            EventVersion = 1;
        }
    }
}
