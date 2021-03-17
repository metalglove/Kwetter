using Kwetter.Services.Common.EventBus.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IIntegrationEventService"/> interface.
    /// </summary>
    public interface IIntegrationEventService
    {
        /// <summary>
        /// Publishes integration events through the event bus asynchronously.
        /// </summary>
        /// <param name="transactionId">The database transaction id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken);

        /// <summary>
        /// Adds and saves an integration event asynchronously.
        /// </summary>
        /// <param name="event">The integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task AddAndSaveEventAsync(IntegrationEvent @event, CancellationToken cancellationToken);
    }
}
