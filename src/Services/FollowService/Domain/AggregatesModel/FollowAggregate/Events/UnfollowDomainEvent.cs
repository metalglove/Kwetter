using System;
using Kwetter.Services.Common.Domain.Events;

namespace Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UnfollowDomainEvent"/> record.
    /// </summary>
    public record UnfollowDomainEvent : DomainEvent
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
        /// Initializes a new instance of the <see cref="UnfollowDomainEvent"/> record.
        /// </summary>
        /// <param name="followingId">The following id.</param>
        /// <param name="followerId">The follower id.</param>
        public UnfollowDomainEvent(Guid followingId, Guid followerId)
        {
            FollowingId = followingId;
            FollowerId = followerId;
            UnfollowedDateTime = DateTime.UtcNow;
            Version = 1;
        }
    }
}