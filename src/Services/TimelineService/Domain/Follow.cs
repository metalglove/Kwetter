using System;

namespace Kwetter.Services.TimelineService.Domain
{
    /// <summary>
    /// Represents the <see cref="Follow"/> class.
    /// </summary>
    public class Follow
    {
        /// <summary>
        /// Gets and sets the following id.
        /// </summary>
        public Guid FollowingId { get; set; }

        /// <summary>
        /// Gets and sets the follower id.
        /// </summary>
        public Guid FollowerId { get; set; }

        /// <summary>
        /// Gets and sets the follow date time.
        /// </summary>
        public DateTime FollowDateTime { get; set; }
    }
}
