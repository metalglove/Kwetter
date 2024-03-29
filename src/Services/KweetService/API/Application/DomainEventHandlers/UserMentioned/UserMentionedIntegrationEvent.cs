﻿using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.KweetService.API.Application.DomainEventHandlers.UserMentioned
{
    /// <summary>
    /// Represents the <see cref="UserMentionedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserMentionedIntegrationEvent : IntegrationEvent
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
        /// Initializes a new instance of the <see cref="UserMentionedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="mentionedByUserId">The user id mentioning this user.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="mentionedDateTime">The mentioned date time.</param>
        public UserMentionedIntegrationEvent(Guid kweetId, Guid userId, Guid mentionedByUserId, string userName, DateTime mentionedDateTime)
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
