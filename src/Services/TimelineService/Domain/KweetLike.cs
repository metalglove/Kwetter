using System;

namespace Kwetter.Services.TimelineService.Domain
{
    /// <summary>
    /// Gets and sets the <see cref="KweetLike"/> class.
    /// </summary>
    public class KweetLike
    {
        /// <summary>
        /// Gets and sets the kweet like id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the liked date time.
        /// </summary>
        public DateTime LikedDateTime { get; set; }
    }
}
