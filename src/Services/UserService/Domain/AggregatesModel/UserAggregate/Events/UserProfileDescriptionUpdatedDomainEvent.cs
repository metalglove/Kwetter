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
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the user profile description.
        /// </summary>
        public string ProfileDescription { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileDescriptionUpdatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="profileDescription">The user profile description.</param>
        public UserProfileDescriptionUpdatedDomainEvent(Guid userId, string profileDescription)
        {
            UserId = userId;
            ProfileDescription = profileDescription;
            EventVersion = 1;
        }
    }
}
