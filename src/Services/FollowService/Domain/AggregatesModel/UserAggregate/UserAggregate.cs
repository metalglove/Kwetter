using Kwetter.Services.Common.Domain;
using Kwetter.Services.FollowService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="UserAggregate"/> class.
    /// The user aggregate is the aggregate root for the FollowService.
    /// </summary>
    public class UserAggregate : Entity, IAggregateRoot
    {
        private List<Follow> followers;
        private List<Follow> followings;
        private string userDisplayName;
        private string userProfilePictureUrl;
        private string userName;

        /// <summary>
        /// Gets and sets the user display name.
        /// </summary>
        public string UserDisplayName 
        { 
            get => userDisplayName; 
            private set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new FollowDomainException("The user display name is null, empty or contains only whitespaces.");
                if (value.Length > 64)
                    throw new FollowDomainException("The user display name length exceeded 64 characters.");
                userDisplayName = value;
            } 
        }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName
        {
            get => userName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new FollowDomainException("The user name is null, empty or contains only whitespaces.");
                if (value.Length > 32)
                    throw new FollowDomainException("The user name length exceeded 32 characters.");
                if (!value.All(char.IsLetterOrDigit))
                    throw new FollowDomainException("The user name is not alphanumeric.");
                userName = value;
            }
        }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl 
        { 
            get => userProfilePictureUrl; 
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new FollowDomainException("The user profile picture url is null, empty or contains only whitespaces.");
                userProfilePictureUrl = value;
            } 
        }

        /// <summary>
        /// Gets a read only collection of followers.
        /// </summary>
        public virtual IReadOnlyCollection<Follow> Followers => followers.AsReadOnly();

        /// <summary>
        /// Gets a read only collection of followings.
        /// </summary>
        public virtual IReadOnlyCollection<Follow> Followings => followings.AsReadOnly();

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected UserAggregate()
        {
            followers = new List<Follow>();
            followings = new List<Follow>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAggregate"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userDisplayName">The user display name.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="userProfilePictureUrl">The user profile picture url.</param>
        public UserAggregate(Guid userId, string userDisplayName, string userName, string userProfilePictureUrl) : this()
        {
            if (userId == Guid.Empty)
                throw new FollowDomainException("The user id is empty.");
            Id = userId;
            UserDisplayName = userDisplayName;
            UserName = userName;
            UserProfilePictureUrl = userProfilePictureUrl;
        }

        /// <summary>
        /// Updates the user display name.
        /// </summary>
        /// <param name="userDisplayName">The user display name.</param>
        public void UpdateUserDisplayName(string userDisplayName)
        {
            UserDisplayName = userDisplayName;
        }

        /// <summary>
        /// Updates the user profile picture url.
        /// </summary>
        /// <param name="userProfilePictureUrl">The user profile picture url.</param>
        public void UpdateUserProfilePictureUrl(string userProfilePictureUrl)
        {
            UserProfilePictureUrl = userProfilePictureUrl;
        }

        /// <summary>
        /// Follows the other user.
        /// </summary>
        /// <param name="otherUser">The other user.</param>
        /// <returns>Returns a boolean indicating whether the other user was followed.</returns>
        public bool Follow(UserAggregate otherUser)
        {
            if (otherUser == default)
                throw new FollowDomainException("The user to be followed can not be null.");
            if (otherUser.Id == Id)
                throw new FollowDomainException("The user to be followed can not be the same user.");
            Follow follow = followings.Find(follow => follow.FollowerId == Id && follow.FollowingId == otherUser.Id);
            if (follow != default)
                return false;
            follow = new(this, otherUser);
            followings.Add(follow);
            otherUser.AddFollower(follow);
            return true;
        }

        /// <summary>
        /// Unfollows the other user.
        /// </summary>
        /// <param name="otherUser">The other user.</param>
        /// <returns>Returns a boolean indicating whether the other user was unfollowed.</returns>
        public bool Unfollow(UserAggregate otherUser)
        {
            if (otherUser == default)
                throw new FollowDomainException("The user to be unfollowed can not be null.");
            if (otherUser.Id == Id)
                throw new FollowDomainException("The user to be unfollowed can not be the same user.");
            Follow follow = followings.Find(follow => follow.FollowerId == Id && follow.FollowingId == otherUser.Id);
            if (follow == default)
                return false;
            if (follow.Unfollow())
            {
                followings.Remove(follow);
                otherUser.RemoveFollower(follow);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the follow from the followers collection.
        /// </summary>
        /// <param name="follow">The follow.</param>
        /// <returns>Returns a boolean indicating whether the follow was removed.</returns>
        private bool RemoveFollower(Follow follow)
        {
            return followers.Remove(follow);
        }

        /// <summary>
        /// Adds the follow to the followers collection.
        /// </summary>
        /// <param name="follow">The follow.</param>
        private void AddFollower(Follow follow)
        {
            followers.Add(follow);
        }
    }
}
