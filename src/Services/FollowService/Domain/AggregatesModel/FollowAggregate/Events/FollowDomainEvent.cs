using System;
using Kwetter.Services.Common.Domain.Events;

namespace Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="FollowDomainEvent"/> class.
    /// </summary>
    public sealed class FollowDomainEvent : DomainEvent
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
        /// Gets and sets the date time when the following was followed.
        /// </summary>
        public DateTime FollowedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowDomainEvent"/> class.
        /// </summary>
        /// <param name="followingId">The following id.</param>
        /// <param name="followerId">The follower id.</param>
        /// <param name="followedDateTime">The follow date time.</param>
        public FollowDomainEvent(Guid followingId, Guid followerId, DateTime followedDateTime)
        {
            FollowingId = followingId;
            FollowerId = followerId;
            FollowedDateTime = followedDateTime;
            EventVersion = 1;
        }
    }
}