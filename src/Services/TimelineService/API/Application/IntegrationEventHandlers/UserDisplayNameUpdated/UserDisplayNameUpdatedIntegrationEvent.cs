using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserDisplayNameUpdated
{
    /// <summary>
    /// Represents the <see cref="UserDisplayNameUpdatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserDisplayNameUpdatedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the display name.
        /// </summary>
        public string UserDisplayName { get; set; }
    }
}
