using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserProfileCreatedDomainEvent"/> class.
    /// </summary>
    public sealed class UserProfileCreatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public UserProfileCreatedDomainEvent(Guid userId)
        {
            UserId = userId;
            EventVersion = 1;
        }
    }
}
