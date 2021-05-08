using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.TimelineService.Domain;
using Kwetter.Services.TimelineService.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserCreated
{
    /// <summary>
    /// Represents the <see cref="UserCreatedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserCreatedIntegrationEventHandler : KwetterEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly ITimelineGraphStore _timelineGraphStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="timelineGraphStore">The timeline graph store.</param>
        public UserCreatedIntegrationEventHandler(ITimelineGraphStore timelineGraphStore)
        {
            _timelineGraphStore = timelineGraphStore ?? throw new ArgumentNullException(nameof(timelineGraphStore));
        }

        /// <summary>
        /// Handles the user created integration event from the user service asynchronously.
        /// </summary>
        /// <param name="event">The user created integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            User user = new()
            {
                Id = @event.UserId,
                UserDisplayName = @event.UserDisplayName,
                UserName = @event.UserName,
                UserProfilePictureUrl = @event.UserProfilePictureUrl
            };
            bool success = await _timelineGraphStore.CreateUserAsync(user);
            if (!success)
                throw new TimelineIntegrationException("Failed to handle UserCreatedIntegrationEvent.");
        }
    }
}
