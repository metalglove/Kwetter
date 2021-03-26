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
        /// Gets and sets the name of the user.
        /// </summary>
        public string Username { get; private set; }

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
        /// <param name="username">The user name.</param>
        /// <param name="description">The profile description.</param>
        public UserAggregate(Guid userId, string username, string description)
        {
            SetId(userId);
            AddDomainEvent(new UserCreatedDomainEvent(Id));
            SetUsername(username);
            Profile = new UserProfile(Id, description);
        }

        /// <summary>
        /// Sets the username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <exception cref="UserDomainException">Thrown when the provided username exceeds 64 characters or is empty.</exception>
        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new UserDomainException("The username is null, empty or contains only whitespaces.");
            if (username?.Length > 64)
                throw new UserDomainException("The length of the username exceeded 64 characters.");
            Username = username;
            AddDomainEvent(new UserUsernameUpdatedDomainEvent(Id, Username));
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
