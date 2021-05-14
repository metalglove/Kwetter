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
        /// Gets the kweet id.
        /// </summary>
        public Guid KweetId { get; }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the user id by whom the user is mentioned.
        /// </summary>
        public Guid MentionedByUserId { get; }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets the date time when the kweet was created.
        /// </summary>
        public DateTime MentionedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="mentionedByUserId">The user id that mentioned this user.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="mentionedDateTime">The mentioned date time.</param>
        public UserMentionedDomainEvent(Guid kweetId, Guid userId, Guid mentionedByUserId, string userName, DateTime mentionedDateTime)
        {
            KweetId = kweetId;
            UserId = userId;
            MentionedByUserId = mentionedByUserId;
            UserName = userName;
            MentionedDateTime = mentionedDateTime;
            EventVersion = 1;
        }
    }
}
