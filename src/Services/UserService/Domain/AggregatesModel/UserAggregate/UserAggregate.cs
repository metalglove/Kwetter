using Kwetter.Services.Common.Domain;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.UserService.Domain.Exceptions;
using System;
using System.Linq;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="UserAggregate"/> class.
    /// The user aggregate is the aggregate root for the UserService.
    /// </summary>
    public class UserAggregate : Entity, IAggregateRoot
    {
        private string displayName;
        private string userName;

        /// <summary>
        /// Gets and sets the display name of the user.
        /// </summary>
        public string DisplayName
        {
            get => displayName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new UserDomainException("The display name is null, empty or contains only whitespaces.");
                if (value.Length > 64)
                    throw new UserDomainException("The length of the display name exceeded 64 characters.");
                displayName = value;
            }
        }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName
        {
            get => userName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new UserDomainException("The user name is null, empty or contains only whitespaces.");
                if (value.Length > 32)
                    throw new UserDomainException("The length of the user name exceeded 32 characters.");
                if (!value.All(char.IsLetterOrDigit))
                    throw new UserDomainException("The user name is not alphanumeric.");
                userName = value;
            }
        }

        /// <summary>
        /// Gets and sets the user profile.
        /// </summary>
        public UserProfile Profile { get; private set; }

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected UserAggregate() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAggregate"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="description">The profile description.</param>
        /// <param name="pictureUrl">The profile picture url.</param>
        public UserAggregate(Guid userId, string displayName, string userName, string description, string pictureUrl)
        {
            if (userId == Guid.Empty)
                throw new UserDomainException("The user id is empty.");
            Id = userId;
            DisplayName = displayName;
            UserName = userName;
            Profile = new UserProfile(Id, description, pictureUrl);
            AddDomainEvent(new UserCreatedDomainEvent(Id, DisplayName, UserName, Profile.Description, Profile.PictureUrl));
        }

        /// <summary>
        /// Sets the display name.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <exception cref="UserDomainException">Thrown when the provided display name exceeds 64 characters or is empty.</exception>
        public void SetDisplayName(string displayName)
        {
            DisplayName = displayName;
            AddDomainEvent(new UserDisplayNameUpdatedDomainEvent(Id, DisplayName));
        }
    }
}
