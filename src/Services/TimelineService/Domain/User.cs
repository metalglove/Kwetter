using System;

namespace Kwetter.Services.TimelineService.Domain
{
    /// <summary>
    /// Represents the <see cref="User"/> class.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the user display name.
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the user profile description.
        /// </summary>
        public string UserProfileDescription { get; set; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl { get; set; }
    }
}
