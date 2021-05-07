using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.KweetService.API.Application.DomainEventHandlers.KweetUnliked
{
    /// <summary>
    /// Represents the <see cref="KweetUnlikedIntegrationEvent"/> class.
    /// </summary>
    public sealed class KweetUnlikedIntegrationEvent : IntegrationEvent
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
        /// Gets the date time when the kweet was unliked.
        /// </summary>
        public DateTime UnlikedDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetUnlikedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="unlikedDateTime">The unliked datetime.</param>
        public KweetUnlikedIntegrationEvent(Guid kweetId, Guid userId, DateTime unlikedDateTime)
        {
            KweetId = kweetId;
            UserId = userId;
            UnlikedDateTime = unlikedDateTime;
            EventVersion = 1;
        }
    }
}
