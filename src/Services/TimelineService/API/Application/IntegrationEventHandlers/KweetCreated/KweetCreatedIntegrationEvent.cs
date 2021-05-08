using Kwetter.Services.Common.Application.Eventing.Integration;
using System;
using System.Collections.Generic;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.KweetCreated
{
    /// <summary>
    /// Represents the <see cref="KweetCreatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class KweetCreatedIntegrationEvent : IncomingIntegrationEvent
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
        /// Gets and sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets and sets the list of hash tags.
        /// </summary>
        public IEnumerable<string> HashTags { get; set; }

        /// <summary>
        /// Gets and sets the list of mentioned users.
        /// </summary>
        public IEnumerable<string> Mentions { get; set; }

        /// <summary>
        /// Gets and sets the date time when the kweet was created.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }
    }
}
