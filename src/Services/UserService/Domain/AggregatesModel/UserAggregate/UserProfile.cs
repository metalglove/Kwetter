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

        /// <summary>
        /// Gets and sets the picture url.
        /// </summary>
        public string PictureUrl { get; private set; }

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected UserProfile() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="description">The description.</param>
        /// <param name="pictureUrl">The picture url.</param>
        public UserProfile(Guid userId, string description, string pictureUrl)
        {
            AddDomainEvent(new UserProfileCreatedDomainEvent(userId));
            SetDescription(userId, description);
            SetPictureUrl(userId, pictureUrl);
        }

        /// <summary>
        /// Sets the profile description.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="UserDomainException">Thrown when the provided description exceeds 512 characters.</exception>
        public void SetDescription(Guid userId, string description)
        {
            if (description?.Length > 512)
                throw new UserDomainException("The length of the profile description exceeded 512.");
            Description = description;
            AddDomainEvent(new UserProfileDescriptionUpdatedDomainEvent(userId, description));
        }

        /// <summary>
        /// Sets the profile picture url.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="pictureUrl">The picture url.</param>
        /// <exception cref="UserDomainException">Thrown when the provided picture url is empty.</exception>
        public void SetPictureUrl(Guid userId, string pictureUrl)
        {
            if (string.IsNullOrWhiteSpace(pictureUrl))
                throw new UserDomainException("The profile picture url is null, empty or contains only white spaces.");
            PictureUrl = pictureUrl;
            AddDomainEvent(new UserProfilePictureUrlUpdatedDomainEvent(userId, pictureUrl));
        }
    }
}
