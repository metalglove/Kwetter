using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.KweetService.API.Application.IntegrationEventHandlers.UserCreated
{
    /// <summary>
    /// Represents the <see cref="UserCreatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserCreatedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the user display name.
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the user profile description.
        /// </summary>
        public string UserProfileDescription { get; set; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl { get; set; }
    }
}
