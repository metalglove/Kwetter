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
    }
}
