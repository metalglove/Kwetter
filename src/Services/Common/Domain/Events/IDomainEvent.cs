using Kwetter.Services.Common.EventBus.Abstractions;
using MediatR;
using System;

namespace Kwetter.Services.Common.Domain.Events
{
    /// <summary>
    /// Represents the <see cref="DomainEvent"/> class.
    /// </summary>
    public abstract record DomainEvent : INotification, IEvent
    {
        /// <summary>
        /// Gets and sets the domain event id.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets and sets the domain event name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets and sets the domain event version.
        /// </summary>
        public int Version { get; init; }

        /// <summary>
        /// Gets and sets the domain event creation date time.
        /// </summary>
        public DateTime CreationDateTime { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent"/> record.
        /// </summary>
        public DomainEvent()
        {
            Id = Guid.NewGuid();
            Name = GetType().FullName;
            CreationDateTime = DateTime.UtcNow;
        }
    }
}
