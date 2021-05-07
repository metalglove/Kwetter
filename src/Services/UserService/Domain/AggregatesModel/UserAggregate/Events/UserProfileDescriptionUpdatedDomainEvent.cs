using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserProfileDescriptionUpdatedDomainEvent"/> class.
    /// </summary>
    public sealed class UserProfileDescriptionUpdatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the user profile description.
        /// </summary>
        public string UserProfileDescription { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileDescriptionUpdatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="profileDescription">The user profile description.</param>
        public UserProfileDescriptionUpdatedDomainEvent(Guid userId, string profileDescription)
        {
            UserId = userId;
            UserProfileDescription = profileDescription;
            EventVersion = 1;
        }
    }
}
