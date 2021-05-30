using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.NotificationService.Infrastructure.Stores;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.API.Application.IntegrationEventHandlers.UserMentioned
{
    /// <summary>
    /// Represents the <see cref="UserMentionedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class UserMentionedIntegrationEventHandler : KwetterEventHandler<UserMentionedIntegrationEvent>
    {
        private readonly INotificationStore _notificationStore;
        private readonly IEventSerializer _eventSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMentionedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="notificationStore">The notification store.</param>
        /// <param name="eventSerializer">The event serializer.</param>
        public UserMentionedIntegrationEventHandler(INotificationStore notificationStore, IEventSerializer eventSerializer)
        {
            _notificationStore = notificationStore ?? throw new ArgumentNullException(nameof(notificationStore));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
        }

        /// <summary>
        /// Handles the user mentioned integration event from the kweet service asynchronously.
        /// </summary>
        /// <param name="event">The user mentioned integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(UserMentionedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            if (@event.UserId == @event.MentionedByUserId)
                return;

            string userId = @event.UserId.ToString();
            string message = _eventSerializer.SerializeToString(@event);

            bool success = await _notificationStore.AppendNotificationAsync(userId, message, cancellationToken);
            if (!success)
                throw new NotificationIntegrationException("UserMentionedIntegrationEvent failed to save unhandled notification to notification store.");

            await _notificationStore.PublishNotificationAsync(userId, message);
        }
    }
}
