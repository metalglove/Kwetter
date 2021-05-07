using System;

namespace Kwetter.Services.UserService.API.Application
{
    /// <summary>
    /// Represents the <see cref="UserIntegrationException"/> class.
    /// </summary>
    public sealed class UserIntegrationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserIntegrationException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UserIntegrationException(string message) : base(message)
        {
        }
    }
}
