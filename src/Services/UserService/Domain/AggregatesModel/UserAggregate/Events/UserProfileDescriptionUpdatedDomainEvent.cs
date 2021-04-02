using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserProfileDescriptionUpdatedDomainEvent"/> record.
    /// </summary>
    public record UserProfileDescriptionUpdatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the user profile profileDescription.
        /// </summary>
        public string ProfileDescription { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileDescriptionUpdatedDomainEvent"/> record.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="profileDescription">The user profile profileDescription.</param>
        public UserProfileDescriptionUpdatedDomainEvent(Guid userId, string profileDescription)
        {
            UserId = userId;
            ProfileDescription = profileDescription;
            Version = 1;
        }
    }
}
