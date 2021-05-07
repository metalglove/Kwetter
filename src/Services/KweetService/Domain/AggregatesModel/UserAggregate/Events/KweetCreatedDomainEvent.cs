using Kwetter.Services.Common.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="KweetCreatedDomainEvent"/> class.
    /// </summary>
    public sealed class KweetCreatedDomainEvent : DomainEvent
    {
        private readonly HashSet<HashTag> hashTags;
        private readonly HashSet<Mention> mentions;

        /// <summary>
        /// Gets the kweet id.
        /// </summary>
        public Guid KweetId { get; }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the list of hash tags.
        /// </summary>
        public IReadOnlyList<string> HashTags => hashTags.Select(ht => ht.Tag).ToList();

        /// <summary>
        /// Gets the list of hash tags.
        /// </summary>
        public IReadOnlyList<string> Mentions => mentions.Select(ht => ht.UserName).ToList();

        /// <summary>
        /// Gets the date time when the kweet was created.
        /// </summary>
        public DateTime CreatedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="message">The message.</param>
        /// <param name="hashTags">The hash tags.</param>
        /// <param name="mentions">The mentions.</param>
        /// <param name="createdDateTime">The created date time.</param>
        public KweetCreatedDomainEvent(Guid kweetId, Guid userId, string message, ref HashSet<HashTag> hashTags, ref HashSet<Mention> mentions, DateTime createdDateTime)
        {
            KweetId = kweetId;
            UserId = userId;
            Message = message;
            this.hashTags = hashTags;
            this.mentions = mentions;
            CreatedDateTime = createdDateTime;
            EventVersion = 1;
        }
    }
}
