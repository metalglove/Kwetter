using Kwetter.Services.Common.Application.Eventing.Integration;

namespace Kwetter.Services.NotificationService.API.Application.Handlers.Events
{
    /// <summary>
    /// Represents the <see cref="UserUnauthorizedEvent"/> class.
    /// </summary>
    public sealed class UserUnauthorizedEvent : IntegrationEvent
    {
        /// <summary>
        /// Gets and sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUnauthorizedEvent"/> class.
        /// </summary>
        public UserUnauthorizedEvent()
        {
            EventVersion = 1;
        }
    }
}
