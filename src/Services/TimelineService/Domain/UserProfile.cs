using System;

namespace Kwetter.Services.TimelineService.Domain
{
    /// <summary>
    /// Represents the <see cref="UserProfile"/> class.
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Gets and sets the user profile id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the user profile description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
