using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserCreatedDomainEvent"/> record.
    /// </summary>
    public record UserCreatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedDomainEvent"/> record.
        /// </summary>
        /// <param name="id">The user id.</param>
        public UserCreatedDomainEvent(Guid id)
        {
            UserId = id;
            Version = 1;
        }
    }
}
