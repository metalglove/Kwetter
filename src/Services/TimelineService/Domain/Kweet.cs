using System;
using System.Collections.Generic;

namespace Kwetter.Services.TimelineService.Domain
{
    /// <summary>
    /// Represents the <see cref="Kweet"/> class.
    /// </summary>
    public class Kweet
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
        /// Gets and sets the kweet message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets and sets the created date time of the kweet.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets and sets the user mentions in the kweet.
        /// </summary>
        public IEnumerable<string> Mentions { get; set; }

        /// <summary>
        /// Gets and sets the hash tags in the kweet.
        /// </summary>
        public IEnumerable<string> HashTags { get; set; }
    }
}
