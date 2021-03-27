using System;
using Kwetter.Services.Common.Domain;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate.Events;
using Kwetter.Services.FollowService.Domain.Exceptions;

namespace Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate
{
    /// <summary>
    /// Represents the <see cref="FollowAggregate"/> class.
    /// The follow aggregate is the aggregate root for the FollowService.
    /// </summary>
    public class FollowAggregate : Entity, IAggregateRoot
    {
        private bool _unfollowed;
        
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
        public DateTime FollowDateTime { get; private set; }
        
        /// <summary>
        /// EF constructor...
        /// </summary>
        protected FollowAggregate() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowAggregate"/> class.
        /// </summary>
        /// <param name="followingId">The following id.</param>
        /// <param name="followerId">The follower id.</param>
        public FollowAggregate(Guid followingId, Guid followerId)
        {
            SetFollowingId(followingId);
            SetFollowerId(followerId);
            FollowDateTime = DateTime.UtcNow;
            AddDomainEvent(new FollowDomainEvent(followingId, followerId, FollowDateTime));
        }
        
        /// <summary>
        /// Sets the follower id of the follow.
        /// </summary>
        /// <param name="followerId">The follower id.</param>
        /// <exception cref="FollowDomainException">Thrown when the provided follower id is empty or malformed.</exception>
        private void SetFollowerId(Guid followerId)
        {
            if (followerId == Guid.Empty)
                throw new FollowDomainException("The follower id is empty.");
            FollowerId = followerId;
        }
       
        /// <summary>
        /// Sets the following id of the follow.
        /// </summary>
        /// <param name="followingId">The following id.</param>
        /// <exception cref="FollowDomainException">Thrown when the provided following id is empty or malformed.</exception>
        private void SetFollowingId(Guid followingId)
        {
            if (followingId == Guid.Empty)
                throw new FollowDomainException("The following id is empty.");
            FollowingId = followingId;
        }

        /// <summary>
        /// Unfollows the following.
        /// </summary>
        /// <exception cref="FollowDomainException">Thrown when the following was already unfollowed in the current tracked context.</exception>
        /// <returns>Returns true only when it succeeds to unfollow the user; otherwise, it throws an exception.</returns>
        public bool Unfollow()
        {
            if (_unfollowed)
                throw new FollowDomainException("The following is already unfollowed.");
            _unfollowed = true;
            AddDomainEvent(new UnfollowDomainEvent(FollowingId, FollowerId));
            return _unfollowed;
        }
    }
}