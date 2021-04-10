using Kwetter.Services.Common.Domain.Exceptions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Kwetter.Services.AuthorizationService.Tests")]
namespace Kwetter.Services.AuthorizationService.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="AuthorizationDomainException"/> class.
    /// </summary>
    internal sealed class AuthorizationDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationDomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        internal AuthorizationDomainException(string message) : base(message)
        {
        }
    }
}
