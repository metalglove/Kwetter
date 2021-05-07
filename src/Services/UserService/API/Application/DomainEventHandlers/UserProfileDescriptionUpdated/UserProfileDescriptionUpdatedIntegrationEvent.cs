using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers.UserProfileDescriptionUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfileDescriptionUpdatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserProfileDescriptionUpdatedIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets and sets the user profile description.
        /// </summary>
        public string UserProfileDescription { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileDescriptionUpdatedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="profileDescription">The user profile description.</param>
        public UserProfileDescriptionUpdatedIntegrationEvent(Guid userId, string profileDescription)
        {
            UserId = userId;
            UserProfileDescription = profileDescription;
            EventVersion = 1;
        }
    }
}
