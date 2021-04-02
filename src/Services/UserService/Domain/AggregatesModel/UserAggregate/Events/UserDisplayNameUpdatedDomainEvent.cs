using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserDisplayNameUpdatedDomainEvent"/> record.
    /// </summary>
    public record UserDisplayNameUpdatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the display name.
        /// </summary>
        public string DisplayName { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisplayNameUpdatedDomainEvent"/> record.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="displayName">The display name.</param>
        public UserDisplayNameUpdatedDomainEvent(Guid userId, string displayName)
        {
            UserId = userId;
            DisplayName = displayName;
            Version = 1;
        }
    }
}
