using System;

namespace Kwetter.Services.TimelineService.API.Application.Queries
{
    /// <summary>
    /// Represents the <see cref="KweetDto"/> class.
    /// </summary>
    public record KweetDto
    {
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets whether the user has liked the kweet.
        /// </summary>
        public bool Liked { get; set; }

        /// <summary>
        /// Gets and sets the like count.
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// Gets and sets the kweet message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets and sets the created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }
    }
}
