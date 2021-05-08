using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserDisplayNameUpdated
{
    /// <summary>
    /// Represents the <see cref="UserDisplayNameUpdatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserDisplayNameUpdatedIntegrationEventHandler : KwetterEventHandler<UserDisplayNameUpdatedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisplayNameUpdatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public UserDisplayNameUpdatedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the user display name updated integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user display name updated integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserDisplayNameUpdatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            bool success = await _timelineGraphStore.UpdateUserDisplayNameAsync(@event.UserId, @event.UserDisplayName);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle UserDisplayNameUpdatedIntegrationEvent.");
        }
    }
}
