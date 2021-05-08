using Kwetter.Services.Common.Application.Eventing.Integration;
using System;

namespace Kwetter.Services.TimelineService.API.Application.IntegrationEventHandlers.UserUnfollowed
{
    /// <summary>
    /// Represents the <see cref="UserUnfollowedIntegrationEvent"/> class.
    /// </summary>
    public sealed class UserUnfollowedIntegrationEvent : IncomingIntegrationEvent
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
        /// Gets and sets the date time when the following was unfollowed.
        /// </summary>
        public DateTime UnfollowedDateTime { get; set; }
    }
}
