using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Domain;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.KweetCreated
{
    /// <summary>
    /// Represents the <see cref="KweetCreatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class KweetCreatedIntegrationEventHandler : KwetterEventHandler<KweetCreatedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetCreatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public KweetCreatedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the kweet created integration event from the kweet service asynchronously.
        /// </summary>
        /// <param name="event">The kweet created integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(KweetCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            Kweet kweet = new()
            {
                Id = @event.KweetId,
                Message = @event.Message,
                UserId = @event.UserId,
                CreatedDateTime = @event.CreatedDateTime,
                Mentions = @event.Mentions,
                HashTags = @event.HashTags
            };
            bool success = await _timelineGraphStore.CreateKweetAsync(kweet);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle KweetCreatedIntegrationEvent.");
        }
    }
}
