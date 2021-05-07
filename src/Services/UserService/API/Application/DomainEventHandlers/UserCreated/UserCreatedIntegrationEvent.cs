using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers.UserCreated
{
    /// <summary>
    /// Represents the <see cref="UserCreatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserCreatedIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets and sets the user display name.
        /// </summary>
        public string UserDisplayName { get; private set; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets and sets the user profile description.
        /// </summary>
        public string UserProfileDescription { get; private set; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="displayName">The user display name.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="description">The user profile description.</param>
        /// <param name="pictureUrl">The user profile picture url.</param>
        public UserCreatedIntegrationEvent(Guid userId, string displayName, string userName, string description, string pictureUrl)
        {
            UserId = userId;
            UserDisplayName = displayName;
            UserName = userName;
            UserProfileDescription = description;
            UserProfilePictureUrl = pictureUrl;
            EventVersion = 1;
        }
    }
}
