using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UnfollowDomainEvent"/> class.
    /// </summary>
    public sealed class UnfollowDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the following id.
        /// </summary>
        public Guid FollowingId { get; }
        
        /// <summary>
        /// Gets and sets the follower id.
        /// </summary>
        public Guid FollowerId { get; }

        /// <summary>
        /// Gets and sets the date time when the following was unfollowed.
        /// </summary>
        public DateTime UnfollowedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnfollowDomainEvent"/> class.
        /// </summary>
        /// <param name="followingId">The following id.</param>
        /// <param name="followerId">The follower id.</param>
        public UnfollowDomainEvent(Guid followingId, Guid followerId)
        {
            FollowingId = followingId;
            FollowerId = followerId;
            UnfollowedDateTime = DateTime.UtcNow;
            EventVersion = 1;
        }
    }
}