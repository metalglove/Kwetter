using System;
using Kwetter.Services.Common.Domain.Events;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="KweetUnlikedDomainEvent"/> record.
    /// </summary>
    public record KweetUnlikedDomainEvent : DomainEvent
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
        /// Gets and sets the date time when the kweet was unliked.
        /// </summary>
        public DateTime UnlikedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetUnlikedDomainEvent"/> record.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        public KweetUnlikedDomainEvent(Guid kweetId, Guid userId)
        {
            KweetId = kweetId;
            UserId = userId;
            UnlikedDateTime = DateTime.UtcNow;
            Version = 1;
        }
    }
}