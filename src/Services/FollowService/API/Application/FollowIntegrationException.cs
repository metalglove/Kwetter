using System;

namespace Kwetter.Services.FollowService.API.Application
{
    /// <summary>
    /// Represents the <see cref="FollowIntegrationException"/> class.
    /// </summary>
    public sealed class FollowIntegrationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowIntegrationException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public FollowIntegrationException(string message) : base(message)
        {
        }
    }
}
