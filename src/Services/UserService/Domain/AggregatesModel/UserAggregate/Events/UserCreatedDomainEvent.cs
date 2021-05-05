using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserCreatedDomainEvent"/> class.
    /// </summary>
    public sealed class UserCreatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the user display name.
        /// </summary>
        public string UserDisplayName { get; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets and sets the user profile description.
        /// </summary>
        public string UserProfileDescription { get; }

        /// <summary>
        /// Gets and sets the user profile picture url.
        /// </summary>
        public string UserProfilePictureUrl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="displayName">The user display name.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="description">The user profile description.</param>
        /// <param name="pictureUrl">The user profile picture url.</param>
        public UserCreatedDomainEvent(Guid userId, string displayName, string userName, string description, string pictureUrl)
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
