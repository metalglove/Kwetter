using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.KweetService.API.Application.DomainEventHandlers.KweetLiked
{
    /// <summary>
    /// Represents the <see cref="KweetLikedIntegrationEvent"/> class.
    /// </summary>
    public sealed class KweetLikedIntegrationEvent : IntegrationEvent
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
        /// Gets and sets the date time when the kweet was liked.
        /// </summary>
        public DateTime LikedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetLikedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="likedDateTime">The liked date time.</param>
        public KweetLikedIntegrationEvent(Guid kweetId, Guid userId, DateTime likedDateTime)
        {
            KweetId = kweetId;
            UserId = userId;
            LikedDateTime = likedDateTime;
            EventVersion = 1;
        }
    }
}
