using System;
using System.Collections.Generic;

namespace Kwetter.Services.TimelineService.Domain
{
    /// <summary>
    /// Gets and sets the <see cref="Timeline"/> class.
    /// </summary>
    public class Timeline
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Gets and sets the list of timeline kweets.
        /// </summary>
        public List<TimelineKweet> Kweets { get; set; }

        /// <summary>
        /// Gets and sets the page number.
        /// </summary>
        public uint PageNumber { get; set; }

        /// <summary>
        /// Gets and sets the page size.
        /// </summary>
        public uint PageSize { get; set; }
    }
}
