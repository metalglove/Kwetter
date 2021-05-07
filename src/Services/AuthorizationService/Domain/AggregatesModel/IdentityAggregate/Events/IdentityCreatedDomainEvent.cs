using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate.Events
{
    /// <summary>
    /// Represents the <see cref="IdentityCreatedDomainEvent"/> class.
    /// </summary>
    public sealed class IdentityCreatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets and sets the OpenId indentifier.
        /// </summary>
        public string OpenId { get; }

        /// <summary>
        /// Gets and sets the given name.
        /// </summary>
        public string GivenName { get; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets and sets the email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets and sets the profile picture url.
        /// </summary>
        public string ProfilePictureUrl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityCreatedDomainEvent"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="openId">The OpenId identifier.</param>
        /// <param name="givenName">The given name.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="email">The email.</param>
        /// <param name="profilePictureUrl">The profile picture url.</param>
        public IdentityCreatedDomainEvent(Guid userId, string openId, string givenName, string userName, string email, string profilePictureUrl)
        {
            UserId = userId;
            OpenId = openId;
            GivenName = givenName;
            UserName = userName;
            Email = email;
            ProfilePictureUrl = profilePictureUrl;
            EventVersion = 1;
        }
    }
}
