using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="UserProfileCreatedDomainEvent"/> class.
    /// </summary>
    public sealed class UserProfileCreatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets and sets the picture url.
        /// </summary>
        public string PictureUrl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="description">The description.</param>
        /// <param name="pictureUrl">The picture url.</param>
        public UserProfileCreatedDomainEvent(Guid userId, string description, string pictureUrl)
        {
            UserId = userId;
            Description = description;
            PictureUrl = pictureUrl;
            EventVersion = 1;
        }
    }
}
