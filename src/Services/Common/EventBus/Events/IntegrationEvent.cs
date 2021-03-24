using Kwetter.Services.Common.EventBus.Abstractions;
using System;

namespace Kwetter.Services.Common.EventBus.Events
{
    /// <summary>
    /// Represents the <see cref="IntegrationEvent"/> record.
    /// </summary>
    public abstract record IntegrationEvent : IEvent
    {
        /// <inheritdoc cref="IEvent.Id"/>
        public Guid Id { get; private init; }

        /// <inheritdoc cref="IEvent.Name"/>
        public string Name { get; private init; }

        /// <inheritdoc cref="IEvent.Version"/>
        public int Version { get; private init; }

        /// <inheritdoc cref="IEvent.CreationDateTime"/>
        public DateTime CreationDateTime { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEvent"/> record.
        /// </summary>
        /// <param name="id">The integration event id.</param>
        /// <param name="creationDateTime">The creation date time.</param>
        /// <param name="name">The integration event name.</param>
        /// <param name="version">The event version.</param>
        protected IntegrationEvent(Guid id, DateTime creationDateTime, string name, int version)
        {
            Id = id;
            CreationDateTime = creationDateTime;
            Name = name;
            Version = version;
        }
    }
}
