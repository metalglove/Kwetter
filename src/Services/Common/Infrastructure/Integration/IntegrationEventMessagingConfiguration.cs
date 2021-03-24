namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventMessagingConfiguration"/> class.
    /// </summary>
    public sealed class IntegrationEventMessagingConfiguration
    {
        /// <summary>
        /// Gets and sets the message queue name.
        /// </summary>
        public string MessageQueueName { get; set; }

        /// <summary>
        /// Gets and sets the service name.
        /// </summary>
        public string ServiceName { get; set; }
    }
}
