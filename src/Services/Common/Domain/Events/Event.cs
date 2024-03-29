﻿using MediatR;
using System;

namespace Kwetter.Services.Common.Domain.Events
{
    /// <summary>
    /// Represents the <see cref="Event"/> class.
    /// </summary>
    public abstract class Event : INotification
    {
        /// <summary>
        /// Gets and sets the id of the event.
        /// </summary>
        public Guid EventId { get; protected set; }

        /// <summary>
        /// Gets and sets the name of the event.
        /// </summary>
        public string EventName { get; protected set; }

        /// <summary>
        /// Gets and sets the version of the event.
        /// </summary>
        public int EventVersion { get; protected set; }

        /// <summary>
        /// Gets and sets the creation date time.
        /// </summary>
        public DateTime EventCreationDateTime { get; protected set; }
    }
}
