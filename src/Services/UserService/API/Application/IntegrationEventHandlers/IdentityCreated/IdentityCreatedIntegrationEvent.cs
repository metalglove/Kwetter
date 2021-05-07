using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.UserService.API.Application.IntegrationEventHandlers.IdentityCreated
{
    /// <summary>
    /// Represents the <see cref="IdentityCreatedIntegrationEvent"/> class.
    /// </summary>
    public sealed class IdentityCreatedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the profile picture url.
        /// </summary>
        public string ProfilePictureUrl { get; set; }

        /// <summary>
        /// Gets and sets the given name.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; set; }
    }
}
