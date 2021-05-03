using Kwetter.Services.Common.Domain;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.FollowService.Domain.Exceptions;
using System;

namespace Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="Follow"/> class.
    /// </summary>
    public class Follow : Entity
    {
        private bool _unfollowed;
        private Guid followerId;
        private Guid followingId;
        private UserAggregate follower;
        private UserAggregate following;

        /// <summary>
        /// Gets and sets the following id.
        /// </summary>
        public Guid FollowingId => followingId;

        /// <summary>
        /// Gets and sets the following.
        /// </summary>
        public virtual UserAggregate Following 
        {
            get => following;
            private set
            {
                if (value == default)
                    throw new FollowDomainException("The following can not be null.");
                if (value.Id == Guid.Empty)
                    throw new FollowDomainException("The following id is empty.");
                following = value;
                followingId = value.Id;
            }
        }

        /// <summary>
        /// Gets and sets the follower id.
        /// </summary>
        public Guid FollowerId => followerId;

        /// <summary>
        /// Gets and sets the follower.
        /// </summary>
        public virtual UserAggregate Follower
        {
            get => follower;
            private set
            {
                if (value == default)
                    throw new FollowDomainException("The follower can not be null.");
                if (value.Id == Guid.Empty)
                    throw new FollowDomainException("The follower id is empty.");
                follower = value;
                followerId = value.Id;
            }
        }

        /// <summary>
        /// Gets and sets the date time when the following was followed.
        /// </summary>
        public DateTime FollowDateTime { get; private set; }

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected Follow() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Follow"/> class.
        /// </summary>
        /// <param name="follower">The follower.</param>
        /// <param name="following">The following.</param>
        internal Follow(UserAggregate follower, UserAggregate following)
        {
            Follower = follower;
            Following = following;
            if (follower.Id == following.Id)
                throw new FollowDomainException("The follow and following id are the same. One can not follow themself.");
            FollowDateTime = DateTime.UtcNow;
            AddDomainEvent(new FollowDomainEvent(Following.Id, Follower.Id, FollowDateTime));
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
