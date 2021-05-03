using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.FollowService.API.Application.DomainEventHandlers.UserFollowed
{
    /// <summary>
    /// Represents the <see cref="UserFollowedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserFollowedIntegrationEvent : IntegrationEvent
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
        /// Gets and sets the date time when the following was followed.
        /// </summary>
        public DateTime FollowedDateTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFollowedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="followingId">The following id.</param>
        /// <param name="followerId">The follower id.</param>
        /// <param name="followedDateTime">The followed date time.</param>
        public UserFollowedIntegrationEvent(Guid followingId, Guid followerId, DateTime followedDateTime)
        {
            FollowingId = followingId;
            FollowerId = followerId;
            FollowedDateTime = followedDateTime;
            EventVersion = 1;
        }
    }
}
