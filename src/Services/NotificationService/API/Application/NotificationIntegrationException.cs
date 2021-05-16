using System;

namespace Kwetter.Services.NotificationService.API.Application
{
    /// <summary>
    /// Represents the <see cref="NotificationIntegrationException"/> class.
    /// </summary>
    public sealed class NotificationIntegrationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationIntegrationException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public NotificationIntegrationException(string message) : base(message)
        {
        }
    }
}
