using Kwetter.Services.Common.Domain;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.UserService.Domain.Exceptions;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="UserAggregate"/> class.
    /// The user aggregate is the root aggregate for the UserService.
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
            Id = userId == Guid.Empty ? throw new ArgumentNullException(nameof(userId)) : userId;
            AddDomainEvent(new UserCreatedDomainEvent(Id));
            SetUsername(username ?? throw new ArgumentNullException(nameof(username)));
            Profile = new UserProfile(Id, description ?? throw new ArgumentNullException(nameof(description)));
        }

        /// <summary>
        /// Sets the username.
        /// </summary>
        /// <exception cref="UserDomainException">Thrown when the provided username exceeds 64 characters.</exception>
        /// <param name="username">The username.</param>
        public void SetUsername(string username)
        {
            if (username?.Length > 64)
                throw new UserDomainException("The length of the username exceeded 64.");
            Username = username;
            AddDomainEvent(new UserUsernameUpdatedDomainEvent(Id, Username));
        }
    }
}
