using Kwetter.Services.Common.Domain.Exceptions;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("Kwetter.Services.UserService.Tests")]
namespace Kwetter.Services.UserService.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="UserDomainException"/> class.
    /// </summary>
    internal sealed class UserDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        internal UserDomainException(string message) : base(message)
        {
        }
    }
}
