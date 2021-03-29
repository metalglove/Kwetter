using System;
using System.Runtime.CompilerServices;
using Kwetter.Services.Common.Domain.Exceptions;

[assembly: InternalsVisibleTo("Kwetter.Services.FollowService.Tests")]
namespace Kwetter.Services.FollowService.Domain.Exceptions
{
    /// <summary>
    /// Represents the <see cref="FollowDomainException"/> class.
    /// </summary>
    internal sealed class FollowDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowDomainException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        internal FollowDomainException(string message) : base(message)
        {
        }
    }
}