using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.FollowService.API.Application.IntegrationEventHandlers.UserDisplayNameUpdated
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
        /// Gets and sets the user display name.
        /// </summary>
        public string UserDisplayName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisplayNameUpdatedIntegrationEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userDisplayName">The user display name.</param>
        public UserDisplayNameUpdatedIntegrationEvent(Guid userId, string userDisplayName)
        {
            UserId = userId;
            UserDisplayName = userDisplayName;
            EventVersion = 1;
        }
    }
}
