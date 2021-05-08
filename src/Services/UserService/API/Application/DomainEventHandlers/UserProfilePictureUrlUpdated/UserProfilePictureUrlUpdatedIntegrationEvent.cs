using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers.UserProfilePictureUrlUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfilePictureUrlUpdatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserProfilePictureUrlUpdatedIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePictureUrlUpdatedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="profilePictureUrl">The user profile picture url.</param>
        public UserProfilePictureUrlUpdatedIntegrationEvent(Guid userId, string profilePictureUrl)
        {
            UserId = userId;
            UserProfilePictureUrl = profilePictureUrl;
            EventVersion = 1;
        }
    }
}
