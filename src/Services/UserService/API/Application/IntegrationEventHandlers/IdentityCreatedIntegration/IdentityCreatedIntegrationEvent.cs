using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.UserService.API.Application.IntegrationEventHandlers.IdentityCreatedIntegration
{
    /// <summary>
    /// Represents the <see cref="IdentityCreatedIntegrationEvent"/> class.
    /// </summary>
    public class IdentityCreatedIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets and sets the profile picture url.
        /// </summary>
        public string ProfilePictureUrl { get; private set; }

        /// <summary>
        /// Gets and sets the given name.
        /// </summary>
        public string GivenName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityCreatedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="givenName">The given name.</param>
        /// <param name="profilePictureUrl">The profile picture url.</param>
        public IdentityCreatedIntegrationEvent(Guid userId, string givenName, string profilePictureUrl)
        {
            UserId = userId;
            GivenName = givenName;
            ProfilePictureUrl = profilePictureUrl;
            EventVersion = 1;
        }
    }
}
