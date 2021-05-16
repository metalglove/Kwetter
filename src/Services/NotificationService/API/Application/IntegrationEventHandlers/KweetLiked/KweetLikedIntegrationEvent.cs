using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.NotificationService.API.Application.IntegrationEventHandlers.KweetLiked
{
    /// <summary>
    /// Represents the <see cref="KweetLikedIntegrationEvent"/> class.
    /// </summary>
    public sealed class KweetLikedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; set; }

        /// <summary>
        /// Gets and sets the kweet user id.
        /// </summary>
        public Guid KweetUserId { get; set; }
        
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the date time when the kweet was liked.
        /// </summary>
        public DateTime LikedDateTime { get; set; }
    }
}
