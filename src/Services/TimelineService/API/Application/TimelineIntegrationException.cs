using System;

namespace Kwetter.Services.TimelineService.API.Application
{
    /// <summary>
    /// Represents the <see cref="TimelineIntegrationException"/> class.
    /// </summary>
    public sealed class TimelineIntegrationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineIntegrationException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public TimelineIntegrationException(string message) : base(message)
        {
        }
    }
}
