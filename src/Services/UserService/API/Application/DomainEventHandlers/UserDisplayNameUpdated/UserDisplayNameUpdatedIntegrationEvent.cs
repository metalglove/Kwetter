using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.UserService.API.Application.DomainEventHandlers.UserDisplayNameUpdated
{
    /// <summary>
    /// Represents the <see cref="UserDisplayNameUpdatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserDisplayNameUpdatedIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets and sets the display name.
        /// </summary>
        public string UserDisplayName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisplayNameUpdatedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="displayName">The display name.</param>
        public UserDisplayNameUpdatedIntegrationEvent(Guid userId, string displayName)
        {
            UserId = userId;
            UserDisplayName = displayName;
            EventVersion = 1;
        }
    }
}
