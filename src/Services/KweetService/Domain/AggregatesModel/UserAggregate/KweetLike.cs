using Kwetter.Services.Common.Domain;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.KweetService.Domain.Exceptions;
using System;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="KweetLike"/> class.
    /// </summary>
    public class KweetLike : Entity
    {
        private Guid kweetId;
        private Guid userId;

        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId 
        { 
            get => kweetId;
            private set 
            {
                if (value == Guid.Empty)
                    throw new KweetDomainException("The kweet id is empty.");
                kweetId = value;
            } 
        }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId 
        { 
            get => userId; 
            private set
            {
                if (value == Guid.Empty)
                    throw new KweetDomainException("The user id is empty.");
                userId = value;
            } 
        }

        /// <summary>
        /// Gets and sets the date time when the like was created.
        /// </summary>
        public DateTime LikedDateTime { get; private set; }

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected KweetLike() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetLike"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        public KweetLike(Guid kweetId, Guid userId)
        {
            KweetId = kweetId;
            UserId = userId;
            LikedDateTime = DateTime.UtcNow;
            AddDomainEvent(new KweetLikedDomainEvent(KweetId, UserId, LikedDateTime));
        }
    }
}
