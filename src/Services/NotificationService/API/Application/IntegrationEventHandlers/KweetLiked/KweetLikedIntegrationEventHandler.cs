using Kwetter.Services.Common.Application.Eventing;
using Kwetter.Services.NotificationService.Infrastructure.Stores;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.API.Application.IntegrationEventHandlers.KweetLiked
{
    /// <summary>
    /// Represents the <see cref="KweetLikedIntegrationEventHandler"/> class.
    /// </summary>
    public sealed class KweetLikedIntegrationEventHandler : KwetterEventHandler<KweetLikedIntegrationEvent>
    {
        private readonly INotificationStore _notificationStore;
        private readonly IEventSerializer _eventSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetLikedIntegrationEventHandler"/> class.
        /// </summary>
        /// <param name="notificationStore">The notification store.</param>
        /// <param name="eventSerializer">The event serializer.</param>
        public KweetLikedIntegrationEventHandler(INotificationStore notificationStore, IEventSerializer eventSerializer)
        {
            _notificationStore = notificationStore ?? throw new ArgumentNullException(nameof(notificationStore));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
        }

        /// <summary>
        /// Handles the kweet liked integration event from the kweet service asynchronously.
        /// </summary>
        /// <param name="event">The kweet liked integration event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public async override ValueTask HandleAsync(KweetLikedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            // if the user who liked the kweet is the same as the user who kweeted it, ignore notification.
            if (@event.UserId == @event.KweetUserId)
                return;

            string userId = @event.KweetUserId.ToString();
            string message = _eventSerializer.SerializeToString(@event);

            bool success = await _notificationStore.AppendNotificationAsync(userId, message, cancellationToken);
            if (!success)
                throw new NotificationIntegrationException("KweetLikedIntegrationEventHandler failed to save unhandled notification to notification store.");

            await _notificationStore.PublishNotificationAsync(userId, message);
        }
    }
}
