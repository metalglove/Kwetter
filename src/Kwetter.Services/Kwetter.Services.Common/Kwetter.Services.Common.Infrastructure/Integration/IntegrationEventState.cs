namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventState"/> enumeration.
    /// </summary>
    public enum IntegrationEventState
    {
        /// <summary>
        /// The integration even is not published.
        /// </summary>
        NotPublished = 0,

        /// <summary>
        /// The integration event is in progress.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The integration event is published.
        /// </summary>
        Published = 2,

        /// <summary>
        /// The integration event has failed to be published.
        /// </summary>
        PublishedFailed = 3
    }
}
