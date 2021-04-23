using System;

namespace Kwetter.Services.Common.Domain.Events
{
    /// <summary>
    /// Represents the <see cref="DomainEvent"/> class.
    /// </summary>
    public abstract class DomainEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent"/> class.
        /// </summary>
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            EventName = GetType().Name;
            EventCreationDateTime = DateTime.UtcNow;
        }
    }
}
