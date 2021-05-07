using Kwetter.Services.Common.Domain.Events;
using System;

namespace Kwetter.Services.Common.Application.Eventing.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEvent"/> class.
    /// </summary>
    public abstract class IntegrationEvent : Event
    {
        private string exchangeName;
        private string routingKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
        /// </summary>
        protected IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            EventName = GetType().Name;
            EventCreationDateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the routing key.
        /// </summary>
        /// <returns>Returns the routing key.</returns>
        public string GetRoutingKey() => routingKey;

        /// <summary>
        /// Sets the routing key.
        /// </summary>
        /// <param name="routingKey">The routing key.</param>
        public void SetRoutingKey(string routingKey) => this.routingKey = routingKey;

        /// <summary>
        /// Gets the exchange name.
        /// </summary>
        /// <returns>Returns the exchange name.</returns>
        public string GetExchangeName() => exchangeName;

        /// <summary>
        /// Sets the exchange name.
        /// </summary>
        /// <param name="exchangeName">The exchange name.</param>
        public void SetExchangeName(string exchangeName) => this.exchangeName = exchangeName;
    }
}
