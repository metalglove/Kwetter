using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.NotificationService.API.Application.IntegrationEventHandlers.UserMentioned
{
    /// <summary>
    /// Represents the <see cref="UserMentionedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserMentionedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the user id by whom the user is mentioned.
        /// </summary>
        public Guid MentionedByUserId { get; set; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the date time when the kweet was created.
        /// </summary>
        public DateTime MentionedDateTime { get; set; }
    }
}
