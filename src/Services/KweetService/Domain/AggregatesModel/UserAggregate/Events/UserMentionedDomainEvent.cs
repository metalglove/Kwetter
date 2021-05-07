using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserMentionedDomainEvent"/> class.
    /// </summary>
    public sealed class UserMentionedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets and sets the date time when the kweet was created.
        /// </summary>
        public DateTime MentionedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="mentionedDateTime">The mentioned date time.</param>
        public UserMentionedDomainEvent(Guid kweetId, Guid userId, string userName, DateTime mentionedDateTime)
        {
            KweetId = kweetId;
            UserId = userId;
            UserName = userName;
            MentionedDateTime = mentionedDateTime;
            EventVersion = 1;
        }
    }
}
