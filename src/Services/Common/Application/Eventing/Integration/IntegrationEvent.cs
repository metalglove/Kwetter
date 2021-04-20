using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.Common.Application.Eventing.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEvent"/> class.
    /// </summary>
    public abstract class IntegrationEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
        /// </summary>
        protected IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            EventName = GetType().Name;
            EventCreationDateTime = DateTime.UtcNow;
        }
    }
}
