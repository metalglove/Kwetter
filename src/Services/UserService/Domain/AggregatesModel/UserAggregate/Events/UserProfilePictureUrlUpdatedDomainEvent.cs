using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserProfilePictureUrlUpdatedDomainEvent"/> class.
    /// </summary>
    public sealed class UserProfilePictureUrlUpdatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string ProfilePictureUrl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePictureUrlUpdatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="profilePictureUrl">The user profile picture url.</param>
        public UserProfilePictureUrlUpdatedDomainEvent(Guid userId, string profilePictureUrl)
        {
            UserId = userId;
            ProfilePictureUrl = profilePictureUrl;
            EventVersion = 1;
        }
    }
}
