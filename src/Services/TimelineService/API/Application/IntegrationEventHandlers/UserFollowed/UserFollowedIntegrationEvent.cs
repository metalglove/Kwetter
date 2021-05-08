using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserFollowed
{
    /// <summary>
    /// Represents the <see cref="UserFollowedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserFollowedIntegrationEvent : IncomingIntegrationEvent
    {
        /// <summary>
        /// Gets and sets the following id.
        /// </summary>
        public Guid FollowingId { get; set; }

        /// <summary>
        /// Gets and sets the follower id.
        /// </summary>
        public Guid FollowerId { get; set; }

        /// <summary>
        /// Gets and sets the date time when the following was followed.
        /// </summary>
        public DateTime FollowedDateTime { get; set; }
    }
}
