using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Domain;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.KweetLiked
{
    /// <summary>
    /// Represents the <see cref="KweetLikedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class KweetLikedIntegrationEventHandler : KwetterEventHandler<KweetLikedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetLikedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public KweetLikedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the kweet liked integration event from the kweet service asynchronously.
        /// </summary>
        /// <param name="event">The kweet liked integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(KweetLikedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            KweetLike kweetLike = new()
            {
                KweetId = @event.KweetId,
                UserId = @event.UserId,
                LikedDateTime = @event.LikedDateTime
            };
            bool success = await _timelineGraphStore.CreateKweetLikeAsync(kweetLike);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle KweetLikedIntegrationEvent.");
        }
    }
}
