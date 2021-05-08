using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.KweetUnliked
{
    /// <summary>
    /// Represents the <see cref="KweetUnlikedIntegrationEvent"/> class.
    /// </summary>
    public sealed class KweetUnlikedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the date time when the kweet was unliked.
        /// </summary>
        public DateTime UnlikedDateTime { get; set; }
    }
}
