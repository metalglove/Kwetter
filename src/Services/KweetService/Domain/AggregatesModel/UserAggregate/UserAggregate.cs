using Kwetter.Services.Common.Domain;
using Kwetter.Services.KweetService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="UserAggregate"/> class.
    /// The user aggregate is the aggregate root for the KweetService.
    /// </summary>
    public class UserAggregate : Entity, IAggregateRoot
    {
        private List<Kweet> kweets;
        private List<KweetLike> kweetLikes;
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
                    throw new KweetDomainException("The user display name is null, empty or contains only whitespaces.");
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
                    throw new KweetDomainException("The user name is null, empty or contains only whitespaces.");
                if (value.Length > 32)
                    throw new KweetDomainException("The user name length exceeded 32 characters.");
                if (!value.All(char.IsLetterOrDigit))
                    throw new KweetDomainException("The user name is not alphanumeric.");
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
                    throw new KweetDomainException("The user profile picture url is null, empty or contains only whitespaces.");
                userProfilePictureUrl = value;
            }
        }

        /// <summary>
        /// Gets the read only collection of kweets.
        /// </summary>
        public virtual IReadOnlyCollection<Kweet> Kweets => kweets.AsReadOnly();

        /// <summary>
        /// Gets the read only collection of kweet likes.
        /// </summary>
        public virtual IReadOnlyCollection<KweetLike> KweetLikes => kweetLikes.AsReadOnly();

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected UserAggregate()
        {
            kweets = new List<Kweet>();
            kweetLikes = new List<KweetLike>();
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
                throw new KweetDomainException("The user id is empty.");
            Id = userId;
            UserDisplayName = userDisplayName;
            UserName = userName;
            UserProfilePictureUrl = userProfilePictureUrl;
        }

        /// <summary>
        /// Creates a kweet asynchronously.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="message">The message.</param>
        /// <param name="findUsersByUserNamesAsync">The function to find mentions.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the kweet.</returns>
        public async Task<Kweet> CreateKweetAsync(Guid kweetId, string message, Func<IEnumerable<Mention>, CancellationToken, Task<IEnumerable<Mention>>> findUsersByUserNamesAsync, CancellationToken cancellationToken = default)
        {
            Kweet kweet = new(kweetId, Id, message);
            await kweet.ProcessMentionsAsync(findUsersByUserNamesAsync, cancellationToken);
            kweets.Add(kweet);
            return kweet;
        }

        /// <summary>
        /// Likes the kweet.
        /// </summary>
        /// <param name="kweet">The kweet.</param>
        /// <returns>Returns a boolean indicating whether the kweet was liked.</returns>
        public bool LikeKweet(Kweet kweet)
        {
            KweetLike kweetLike = kweet.AddLike(Id);
            if (kweetLike == default)
                return false;
            kweetLikes.Add(kweetLike);
            return true;
        }

        /// <summary>
        /// Unlikes the kweet.
        /// </summary>
        /// <param name="kweet">The kweet.</param>
        /// <returns>Returns a value indicating whether the kweet was unliked.</returns>
        public bool UnlikeKweet(Kweet kweet)
        {
            KweetLike kweetLike = kweet.RemoveLike(Id);
            if (kweetLike == default)
                return false;
            kweetLikes.Remove(kweetLike);
            return true;
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
    }
}
