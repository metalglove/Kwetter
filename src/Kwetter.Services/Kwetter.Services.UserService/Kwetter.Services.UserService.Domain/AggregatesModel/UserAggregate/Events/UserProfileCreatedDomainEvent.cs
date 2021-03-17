using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserProfileCreatedDomainEvent"/> record.
    /// </summary>
    public record UserProfileCreatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileCreatedDomainEvent"/> record.
        /// </summary>
        /// <param name="userId"></param>
        public UserProfileCreatedDomainEvent(Guid userId)
        {
            UserId = userId;
            Version = 1;
        }
    }
}
