using Kwetter.Services.Common.Domain;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events;
using System;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="Mention"/> class.
    /// </summary>
    public class Mention : Entity
    {
        private readonly Kweet kweet;

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets and sets the user.
        /// </summary>
        public virtual UserAggregate User { get; set; }
        
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; private set; }

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected Mention() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mention"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="kweet">The kweet.</param>
        private Mention(UserAggregate user, Kweet kweet)
        {
            UserName = user.UserName;
            User = user;
            KweetId = kweet.Id;
            AddDomainEvent(new UserMentionedDomainEvent(KweetId, user.Id, UserName, kweet.CreatedDateTime));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mention"/> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="kweet">The kweet.</param>
        internal Mention(string userName, Kweet kweet)
        {
            UserName = userName;
            KweetId = kweet.Id;
            this.kweet = kweet;
        }

        /// <summary>
        /// Creates a trackable mention (including domain event).
        /// </summary>
        /// <param name="user">The user.</param>
        public Mention ToTrackableMention(UserAggregate user)
        {
            return new Mention(user, kweet);
        }
    }
}
