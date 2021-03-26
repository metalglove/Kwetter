using Kwetter.Services.Common.Domain;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate.Events;
using Kwetter.Services.KweetService.Domain.Exceptions;
using System;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate
{
    /// <summary>
    /// Represents the <see cref="KweetLike"/> class.
    /// </summary>
    public class KweetLike : Entity
    {
        /// <summary>
        /// Gets and sets the kweet id.
        /// </summary>
        public Guid KweetId { get; private set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets and sets the date time when the like was created.
        /// </summary>
        public DateTime CreatedDateTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetLike"/> class.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        public KweetLike(Guid kweetId, Guid userId)
        {
            SetKweetId(kweetId);
            SetUserId(userId);
            CreatedDateTime = DateTime.UtcNow;
            AddDomainEvent(new KweetLikedDomainEvent(kweetId, userId, CreatedDateTime));
        }

        /// <summary>
        /// Sets the user id of the like.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <exception cref="KweetDomainException">Thrown when the provided user id is empty or malformed.</exception>
        private void SetUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new KweetDomainException("The user id is empty.");
            UserId = userId;
        }

        /// <summary>
        /// Sets the kweet id of the like.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <exception cref="KweetDomainException">Thrown when the provided kweet id is empty or malformed.</exception>
        private void SetKweetId(Guid kweetId)
        {
            if (kweetId == Guid.Empty)
                throw new KweetDomainException("The kweet id is empty.");
            KweetId = kweetId;
        }
    }
}
