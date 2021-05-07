using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.KweetService.API.Application.IntegrationEventHandlers.UserProfilePictureUrlUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfilePictureUrlUpdatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserProfilePictureUrlUpdatedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl { get; set; }
    }
}
