using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="KweetLikedDomainEvent"/> class.
    /// </summary>
    public sealed class KweetLikedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets the kweet id.
        /// </summary>
        public Guid KweetId { get; }

        /// <summary>
        /// Gets the kweet user id.
        /// </summary>
        public Guid KweetUserId { get; }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the date time when the kweet was liked.
        /// </summary>
        public DateTime LikedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetLikedDomainEvent"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="kweetUserId">The kweet user id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="likedDateTime">The liked date time.</param>
        public KweetLikedDomainEvent(Guid kweetId, Guid kweetUserId, Guid userId, DateTime likedDateTime)
        {
            KweetId = kweetId;
            KweetUserId = kweetUserId;
            UserId = userId;
            LikedDateTime = likedDateTime;
            EventVersion = 1;
        }
    }
}
