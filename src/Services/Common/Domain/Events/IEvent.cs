using System;

namespace Kwetter.Services.Common.Domain.Events
{
    /// <summary>
    /// Represents the <see cref="IEvent"/> interface.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets the id of the event.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the version of the event.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Gets the creation date time.
        /// </summary>
        public DateTime CreationDateTime { get; }
    }
}
