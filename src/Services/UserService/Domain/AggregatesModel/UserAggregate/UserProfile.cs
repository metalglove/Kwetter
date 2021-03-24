using Kwetter.Services.Common.Domain;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.UserService.Domain.Exceptions;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="UserProfile"/> class.
    /// </summary>
    public class UserProfile : Entity
    {
        /// <summary>
        /// Gets and sets the description.
        /// </summary>
        public string Description { get; private set; }

        // TODO: profile picture?

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected UserProfile() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="description">The description.</param>
        public UserProfile(Guid userId, string description)
        {
            AddDomainEvent(new UserProfileCreatedDomainEvent(userId));
            SetDescription(userId, description);
        }

        /// <summary>
        /// Sets the profile description.
        /// </summary>
        /// <exception cref="UserDomainException">Thrown when the provided description exceeds 512 characters.</exception>
        /// <param name="userId">The user id.</param>
        /// <param name="description">The description.</param>
        public void SetDescription(Guid userId, string description)
        {
            if (description?.Length > 512)
                throw new UserDomainException("The length of the profile description exceeded 512.");
            Description = description;
            AddDomainEvent(new UserProfileDescriptionUpdatedDomainEvent(userId, description));
        }
    }
}
