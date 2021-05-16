using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.NotificationService.API.Application
{
    /// <summary>
    /// Represents the <see cref="INotificationPublisher"/> interface.
    /// </summary>
    public interface INotificationPublisher
    {
        /// <summary>
        /// Publishes the notification to all connections of the user connected to this node asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns a boolean indicating whether at least one message was received by the user.</returns>
        public Task<bool> PublishNotificationAsync(string userId, ReadOnlyMemory<byte> message, CancellationToken cancellationToken);
    }
}
