using Kwetter.Services.Common.Domain.Exceptions;
using System;

namespace Kwetter.Services.UserService.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="UserDomainException"/> class.
    /// </summary>
    public sealed class UserDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDomainException"/> class.
        /// </summary>
        public UserDomainException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UserDomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UserDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
