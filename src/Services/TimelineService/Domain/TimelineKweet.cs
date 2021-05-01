using System;

namespace Kwetter.Services.TimelineService.Domain
{
    /// <summary>
    /// Represents the <see cref="TimelineKweet"/> class.
    /// </summary>
    public class TimelineKweet
    {
        /// <summary>
        /// Gets and sets the timeline kweet id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the kweet message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets and sets the user display name.
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        /// Gets and sets a value indicating whether the kweet is liked by the user.
        /// </summary>
        public bool Liked { get; set; }

        /// <summary>
        /// Gets and sets the like count.
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl { get; set; }

        /// <summary>
        /// Gets and sets the create date time of the kweet.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }
    }
}
