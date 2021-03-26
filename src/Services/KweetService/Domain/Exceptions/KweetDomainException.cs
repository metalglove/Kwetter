using Kwetter.Services.Common.Domain.Exceptions;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("Kwetter.Services.KweetService.Tests")]
namespace Kwetter.Services.KweetService.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="KweetDomainException"/> class.
    /// </summary>
    internal sealed class KweetDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KweetDomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        internal KweetDomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetDomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        internal KweetDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
