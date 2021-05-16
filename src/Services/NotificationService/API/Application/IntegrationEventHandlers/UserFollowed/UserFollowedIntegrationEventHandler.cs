using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.NotificationService.Infrastructure.Stores;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.API.Application.IntegrationEventHandlers.UserFollowed
{
    /// <summary>
    /// Represents the <see cref="UserFollowedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserFollowedIntegrationEventHandler : KwetterEventHandler<UserFollowedIntegrationEvent>
    {
        private readonly INotificationStore _notificationStore;
        private readonly IEventSerializer _eventSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFollowedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="notificationStore">The notification store.</param>
        /// <param name="eventSerializer">The event serializer.</param>
        public UserFollowedIntegrationEventHandler(INotificationStore notificationStore, IEventSerializer eventSerializer)
        {
            _notificationStore = notificationStore ?? throw new ArgumentNullException(nameof(notificationStore));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
        }

        /// <summary>
        /// Handles the user followed integration event from the follow service asynchronously.
        /// </summary>
        /// <param name="event">The user followed integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserFollowedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            string userId = @event.FollowingId.ToString();
            string message = _eventSerializer.SerializeToString(@event);

            bool success = await _notificationStore.AppendNotificationAsync(userId, message, cancellationToken);
            if (!success)
                throw new NotificationIntegrationException("UserFollowedIntegrationEvent failed to save unhandled notification to notification store.");

            await _notificationStore.PublishNotificationAsync(userId, message);
        }
    }
}
