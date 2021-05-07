using Kwetter.Services.Common.Domain;
using System;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="HashTag"/> class.
    /// </summary>
    public class HashTag : Entity
    {
        /// <summary>
        /// Gets and sets the tag.
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; private set; }

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected HashTag() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTag"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="kweetId">The kweet id.</param>
        public HashTag(string tag, Guid kweetId)
        {
            Tag = tag;
            KweetId = kweetId;
        }
    }
}
