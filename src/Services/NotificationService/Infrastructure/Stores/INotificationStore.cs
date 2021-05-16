using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.Infrastructure.Stores
{
    /// <summary>
    /// Represents the <see cref="INotificationStore"/> interface.
    /// </summary>
    public interface INotificationStore
    {
        /// <summary>
        /// Gets the notifications for a user asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an asynchronous enumerable of notifications as string.</returns>
        public IAsyncEnumerable<string> GetNotificationsAsync(string userId, CancellationToken cancellationToken);

        /// <summary>
        /// Appends the notification to the user's notifications asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="notification">The notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable boolean indicating whether the notification is successfully appended.</returns>
        public Task<bool> AppendNotificationAsync(string userId, string notification, CancellationToken cancellationToken);

        /// <summary>
        /// Prepends the notifications to the user's notifications asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="notifications">The notifications.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable boolean indicating whether the notifications are successfully prepended.</returns>
        public Task<bool> PrependNotificationsAsync(string userId, IEnumerable<string> notifications, CancellationToken cancellationToken);

        /// <summary>
        /// Publishes a notification on the user's notification channel asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="notification">The notification.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task PublishNotificationAsync(string userId, string notification);

        /// <summary>
        /// Subscribes for notification on the user's notification channel asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="handler">The handler to execute on a notification arriving.</param>
        /// <returns>Returns an awaitable unsubscribe action.</returns>
        public Task<Action> SubscribeNotificationAsync(string userId, Func<string, ValueTask> handler);
    }
}
