using System;

namespace Kwetter.Services.KweetService.API.Application
{
    /// <summary>
    /// Represents the <see cref="KweetIntegrationException"/> class.
    /// </summary>
    public sealed class KweetIntegrationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KweetIntegrationException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public KweetIntegrationException(string message) : base(message)
        {
        }
    }
}
