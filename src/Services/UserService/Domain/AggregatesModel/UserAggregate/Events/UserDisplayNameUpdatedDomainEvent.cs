using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserDisplayNameUpdatedDomainEvent"/> class.
    /// </summary>
    public sealed class UserDisplayNameUpdatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the display name.
        /// </summary>
        public string UserDisplayName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisplayNameUpdatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="displayName">The display name.</param>
        public UserDisplayNameUpdatedDomainEvent(Guid userId, string displayName)
        {
            UserId = userId;
            UserDisplayName = displayName;
            EventVersion = 1;
        }
    }
}
