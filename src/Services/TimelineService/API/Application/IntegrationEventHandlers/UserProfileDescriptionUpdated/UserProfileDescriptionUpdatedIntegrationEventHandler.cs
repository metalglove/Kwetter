using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserProfileDescriptionUpdated
{
    /// <summary>
    /// Represents the <see cref="UserProfileDescriptionUpdatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserProfileDescriptionUpdatedIntegrationEventHandler : KwetterEventHandler<UserProfileDescriptionUpdatedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileDescriptionUpdatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public UserProfileDescriptionUpdatedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the user profile description updated integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user profile description updated integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserProfileDescriptionUpdatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            bool success = await _timelineGraphStore.UpdateUserProfileDescriptionAsync(@event.UserId, @event.UserProfileDescription);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle UserProfileDescriptionUpdatedIntegrationEvent.");
        }
    }
}
