using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate.Events;
using Kwetter.Services.AuthorizationService.Domain.Exceptions;
using Kwetter.Services.Common.Domain;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate
{
    /// <summary>
    /// Represents the <see cref="IdentityAggregate"/> class.
    /// The identity aggregate is the aggregate root for the AuthorizationService.
    /// </summary>
    public class IdentityAggregate : Entity, IAggregateRoot
    {
        private string openId;
        private string givenName;
        private string email;
        private string profilePictureUrl;

        /// <summary>
        /// Gets and sets the OpenId identifier.
        /// </summary>
        public string OpenId 
        { 
            get => openId;
            private set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new AuthorizationDomainException("The open id is null, empty or contains only whitespaces.");
                openId = value;
            } 
        }

        /// <summary>
        /// Gets and sets the given name of the user.
        /// </summary>
        public string GivenName 
        { 
            get => givenName; 
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new AuthorizationDomainException("The given name is null, empty or contains only whitespaces.");
                givenName = value;
            }
        }

        /// <summary>
        /// Gets and sets the email of the user.
        /// </summary>
        public string Email
        {
            get => email;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new AuthorizationDomainException("The email is null, empty or contains only whitespaces.");

                try
                {
                    // Normalize the domain
                    value = Regex.Replace(value, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                    bool isValidEmail = Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                    if (!isValidEmail)
                        throw new AuthorizationDomainException("The email is not a valid email address.");

                    // Examines the domain part of the email and normalizes it.
                    static string DomainMapper(Match match)
                    {
                        // Use IdnMapping class to convert Unicode domain names.
                        IdnMapping idn = new();

                        // Pull out and process domain name (throws ArgumentException on invalid)
                        string domainName = idn.GetAscii(match.Groups[2].Value);

                        return match.Groups[1].Value + domainName;
                    }
                }
                catch (Exception)
                {
                    throw new AuthorizationDomainException("The email is not a valid email address.");
                }

                email = value;
            }
        }

        /// <summary>
        /// Gets and sets the profile picture url of the user.
        /// </summary>
        public string ProfilePictureUrl 
        { 
            get => profilePictureUrl; 
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new AuthorizationDomainException("The profile picture url is null, empty or contains only whitespaces.");
                profilePictureUrl = value;
            }
        }

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected IdentityAggregate() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityAggregate"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="openId">The open id.</param>
        /// <param name="givenName">The given name.</param>
        /// <param name="email">The email.</param>
        /// <param name="profilePictureUrl">The profile picture url.</param>
        public IdentityAggregate(Guid userId, string openId, string givenName, string email, string profilePictureUrl)
        {
            if (userId == Guid.Empty)
                throw new AuthorizationDomainException("The user id is empty.");
            Id = userId;
            OpenId = openId;
            GivenName = givenName;
            Email = email;
            ProfilePictureUrl = profilePictureUrl;
            AddDomainEvent(new IdentityCreatedDomainEvent(Id, OpenId, GivenName, Email, ProfilePictureUrl));
        }
    }
}
