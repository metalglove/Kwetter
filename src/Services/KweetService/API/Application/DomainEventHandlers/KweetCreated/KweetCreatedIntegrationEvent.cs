using Kwetter.Services.Common.Application.Eventing.Integration;
using System;
using System.Collections.Generic;

namespace Kwetter.Services.KweetService.API.Application.DomainEventHandlers.KweetCreated
{
    /// <summary>
    /// Represents the <see cref="KweetCreatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class KweetCreatedIntegrationEvent : IntegrationEvent
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
        /// Gets the message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the list of hash tags.
        /// </summary>
        public IEnumerable<string> HashTags { get; }

        /// <summary>
        /// Gets the list of mentioned users.
        /// </summary>
        public IEnumerable<string> Mentions { get; }

        /// <summary>
        /// Gets the date time when the kweet was created.
        /// </summary>
        public DateTime CreatedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetCreatedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="message">The message.</param>
        /// <param name="hashTags">The hash tags.</param>
        /// <param name="mentions">The mentions.</param>
        /// <param name="createdDateTime">The created date time.</param>
        public KweetCreatedIntegrationEvent(Guid kweetId, Guid userId, IEnumerable<string> hashTags, IEnumerable<string> mentions, string message, DateTime createdDateTime)
        {
            KweetId = kweetId;
            UserId = userId;
            Message = message;
            HashTags = hashTags;
            Mentions = mentions;
            CreatedDateTime = createdDateTime;
            EventVersion = 1;
        }
    }
}
