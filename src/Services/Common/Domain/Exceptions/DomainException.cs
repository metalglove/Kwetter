using System;

namespace Kwetter.Services.Common.Domain.Exceptions
{
    /// <summary>
    /// Represents the base <see cref="DomainException"/> class.
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        protected DomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        protected DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
