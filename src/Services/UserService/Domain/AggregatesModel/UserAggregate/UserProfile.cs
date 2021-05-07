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
        private string description;
        private string pictureUrl;

        /// <summary>
        /// Gets and sets the description.
        /// </summary>
        public string Description
        {
            get => description;
            private set
            {
                if (value?.Length > 512)
                    throw new UserDomainException("The length of the profile description exceeded 512.");
                description = value;
            }
        }

        /// <summary>
        /// Gets and sets the picture url.
        /// </summary>
        public string PictureUrl 
        { 
            get => pictureUrl; 
            private set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new UserDomainException("The profile picture url is null, empty or contains only white spaces.");
                pictureUrl = value;
            } 
        }

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
            Description = description;
            PictureUrl = pictureUrl;
            AddDomainEvent(new UserProfileCreatedDomainEvent(userId, Description, PictureUrl));
        }

        /// <summary>
        /// Updates the profile description.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="UserDomainException">Thrown when the provided description exceeds 512 characters.</exception>
        public void UpdateDescription(Guid userId, string description)
        {
            Description = description;
            AddDomainEvent(new UserProfileDescriptionUpdatedDomainEvent(userId, Description));
        }

        /// <summary>
        /// Updates the profile picture url.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="pictureUrl">The picture url.</param>
        /// <exception cref="UserDomainException">Thrown when the provided picture url is empty.</exception>
        public void UpdatePictureUrl(Guid userId, string pictureUrl)
        {
            PictureUrl = pictureUrl;
            AddDomainEvent(new UserProfilePictureUrlUpdatedDomainEvent(userId, PictureUrl));
        }
    }
}
