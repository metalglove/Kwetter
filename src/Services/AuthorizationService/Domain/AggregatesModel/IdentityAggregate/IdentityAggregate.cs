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
        /// <summary>
        /// Gets and sets the OpenId identifier.
        /// </summary>
        public string OpenId { get; private set; }

        /// <summary>
        /// Gets and sets the given name of the user.
        /// </summary>
        public string GivenName { get; private set; }

        /// <summary>
        /// Gets and sets the email of the user.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets and sets the profile picture url of the user.
        /// </summary>
        public string ProfilePictureUrl { get; private set; }

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
            SetUserId(userId);
            SetOpenId(openId);
            SetGivenName(givenName);
            SetEmail(email);
            SetProfilePictureUrl(profilePictureUrl);
            AddDomainEvent(new IdentityCreatedDomainEvent(Id, OpenId, GivenName, Email, ProfilePictureUrl));
        }

        /// <summary>
        /// Sets the id of the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        private void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new AuthorizationDomainException("The user id is empty.");
            Id = userId;
        }

        /// <summary>
        /// Sets the open id of the user.
        /// </summary>
        /// <param name="openId">The open id.</param>
        /// <exception cref="AuthorizationDomainException">Thrown when the provided open id is null, empty or contains only whitespaces.</exception>

        private void SetOpenId(string openId)
        {
            if (string.IsNullOrWhiteSpace(openId))
                throw new AuthorizationDomainException("The open id is null, empty or contains only whitespaces.");
            OpenId = openId;
        }

        /// <summary>
        /// Sets the email of the user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <exception cref="AuthorizationDomainException">Thrown when the provided email is invalid.</exception>
        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new AuthorizationDomainException("The email is null, empty or contains only whitespaces.");

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
                bool isValidEmail = Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                if (!isValidEmail)
                    throw new AuthorizationDomainException("The email is not a valid email address.");

                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (Exception)
            {
                throw new AuthorizationDomainException("The email is not a valid email address.");
            }
            
            Email = email;
        }

        /// <summary>
        /// Sets the given name of the user.
        /// </summary>
        /// <param name="givenName">The given name.</param>
        /// <exception cref="AuthorizationDomainException">Thrown when the provided given name is null, empty or contains only whitespaces.</exception>
        private void SetGivenName(string givenName)
        {
            if (string.IsNullOrWhiteSpace(givenName))
                throw new AuthorizationDomainException("The given name is null, empty or contains only whitespaces.");
            GivenName = givenName;
        }

        /// <summary>
        /// Sets the profile picture url of the user.
        /// </summary>
        /// <param name="profilePictureUrl">The profile picture url.</param>\
        /// <exception cref="AuthorizationDomainException">Thrown when the provided profile picture url is null, empty or contains only whitespaces.</exception>
        private void SetProfilePictureUrl(string profilePictureUrl)
        {
            if (string.IsNullOrWhiteSpace(profilePictureUrl))
                throw new AuthorizationDomainException("The profile picture url is null, empty or contains only whitespaces.");
            ProfilePictureUrl = profilePictureUrl;
        }
    }
}
