using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserProfileDescriptionUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfileDescriptionUpdatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserProfileDescriptionUpdatedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the user profile description.
        /// </summary>
        public string UserProfileDescription { get; set; }
    }
}
