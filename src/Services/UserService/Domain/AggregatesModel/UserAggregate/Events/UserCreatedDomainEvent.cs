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
        public Guid UserId { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public UserCreatedDomainEvent(Guid userId)
        {
            UserId = userId;
            EventVersion = 1;
        }
    }
}
