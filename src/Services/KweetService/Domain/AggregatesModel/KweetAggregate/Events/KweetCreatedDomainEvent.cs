using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="KweetCreatedDomainEvent"/> record.
    /// </summary>
    public record KweetCreatedDomainEvent : DomainEvent
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
        /// Gets and sets the message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets and sets the date time when the kweet was created.
        /// </summary>
        public DateTime CreatedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetCreatedDomainEvent"/> record.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="message">The message.</param>
        /// <param name="createdDateTime">The created date time.</param>
        public KweetCreatedDomainEvent(Guid kweetId, Guid userId, string message, DateTime createdDateTime)
        {
            KweetId = kweetId;
            UserId = userId;
            Message = message;
            CreatedDateTime = createdDateTime;
            Version = 1;
        }
    }
}
