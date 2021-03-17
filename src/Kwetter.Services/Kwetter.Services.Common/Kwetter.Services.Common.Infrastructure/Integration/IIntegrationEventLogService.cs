using Kwetter.Services.Common.EventBus.Events;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IIntegrationEventLogService"/> interface.
    /// </summary>
    public interface IIntegrationEventLogService
    {
        /// <summary>
        /// Retrieves the event logs pending to be published asynchronously.
        /// </summary>
        /// <param name="transactionId">The database transaction id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable enumerable of the integration event log entries.</returns>
        public ValueTask<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the integration event asynchronously.
        /// </summary>
        /// <param name="event">The integration event.</param>
        /// <param name="transaction">The database transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks the integration event as published asynchronously.
        /// </summary>
        /// <param name="eventId">The integration event id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks the integration event as in progress asynchronously.
        /// </summary>
        /// <param name="eventId">The integration event id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks the integration event as failed to publish asynchronously.
        /// </summary>
        /// <param name="eventId">The integration event id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken = default);
    }
}
