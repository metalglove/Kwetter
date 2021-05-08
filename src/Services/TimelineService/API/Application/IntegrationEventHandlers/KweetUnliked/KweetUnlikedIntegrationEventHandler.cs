using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.KweetUnliked
{
    /// <summary>
    /// Represents the <see cref="KweetUnlikedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class KweetUnlikedIntegrationEventHandler : KwetterEventHandler<KweetUnlikedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetUnlikedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public KweetUnlikedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the kweet unliked integration event from the kweet service asynchronously.
        /// </summary>
        /// <param name="event">The kweet unliked integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(KweetUnlikedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            bool success = await _timelineGraphStore.DeleteKweetLikeAsync(@event.UserId, @event.KweetId);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle KweetUnlikedIntegrationEvent.");
        }
    }
}
