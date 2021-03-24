using Kwetter.Services.Common.EventBus.Events;
using System;
using System.Linq;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventLogEntry"/> class.
    /// </summary>
    public class IntegrationEventLogEntry
    {
        /// <summary>
        /// Gets and sets the event id.
        /// </summary>
        public Guid EventId { get; private set; }

        /// <summary>
        /// Gets and sets the event type name.
        /// </summary>
        public string EventTypeName { get; private set; }

        /// <summary>
        /// Gets the event type short name.
        /// </summary>
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();

        /// <summary>
        /// Gets and sets the integration event.
        /// </summary>
        public IntegrationEvent IntegrationEvent { get; set; }

        /// <summary>
        /// Gets and sets the integration event state.
        /// </summary>
        public IntegrationEventState State { get; set; }

        /// <summary>
        /// Gets and sets the times sent.
        /// </summary>
        public int TimesSent { get; set; }

        /// <summary>
        /// Gets and sets the creation date time.
        /// </summary>
        public DateTime CreationDateTime { get; private set; }

        /// <summary>
        /// Gets and sets the content.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Gets and sets the transaction id.
        /// </summary>
        public Guid TransactionId { get; private set; }

        private IntegrationEventLogEntry() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventLogEntry"/> class.
        /// </summary>
        /// <param name="event">The integration event.</param>
        /// <param name="eventSerialized"></param>
        /// <param name="transactionId">The transaction id.</param>
        public IntegrationEventLogEntry(IntegrationEvent @event, string eventSerialized, Guid transactionId)
        {
            TransactionId = transactionId;
            EventId = @event.Id;
            CreationDateTime = @event.CreationDateTime;
            EventTypeName = @event.GetType().FullName;
            Content = eventSerialized;
            State = IntegrationEventState.NotPublished;
            TimesSent = 0;
        }
    }
}
