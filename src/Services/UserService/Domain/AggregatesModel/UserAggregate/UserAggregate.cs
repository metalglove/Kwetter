using Kwetter.Services.Common.Domain;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.UserService.Domain.Exceptions;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="UserAggregate"/> class.
    /// The user aggregate is the aggregate root for the UserService.
    /// </summary>
    public class UserAggregate : Entity, IAggregateRoot
    {
        /// <summary>
        /// Gets and sets the display name of the user.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets and sets the user profile.
        /// </summary>
        public UserProfile Profile { get; private set; }

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected UserAggregate() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAggregate"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="description">The profile description.</param>
        /// <param name="pictureUrl">The profile picture url.</param>
        public UserAggregate(Guid userId, string displayName, string description, string pictureUrl)
        {
            SetId(userId);
            AddDomainEvent(new UserCreatedDomainEvent(Id));
            SetDisplayName(displayName);
            Profile = new UserProfile(Id, description, pictureUrl);
        }

        /// <summary>
        /// Sets the display name.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <exception cref="UserDomainException">Thrown when the provided display name exceeds 64 characters or is empty.</exception>
        public void SetDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new UserDomainException("The display name is null, empty or contains only whitespaces.");
            if (displayName?.Length > 64)
                throw new UserDomainException("The length of the display name exceeded 64 characters.");
            DisplayName = displayName;
            AddDomainEvent(new UserDisplayNameUpdatedDomainEvent(Id, DisplayName));
        }
        
        /// <summary>
        /// Sets the id of the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <exception cref="UserDomainException">Thrown when the provided user id is empty or malformed.</exception>
        private void SetId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new UserDomainException("The user id is empty.");
            Id = userId;
        }
    }
}
