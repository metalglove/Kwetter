using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserUsernameUpdatedDomainEvent"/> record.
    /// </summary>
    public record UserUsernameUpdatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets and sets the username.
        /// </summary>
        public string Username { get; init; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserUsernameUpdatedDomainEvent"/> record.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username.</param>
        public UserUsernameUpdatedDomainEvent(Guid userId, string username)
        {
            UserId = userId;
            Username = username;
            Version = 1;
        }
    }
}
