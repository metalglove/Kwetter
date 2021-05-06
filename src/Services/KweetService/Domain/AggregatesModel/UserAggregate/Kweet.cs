using Kwetter.Services.Common.Domain;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.KweetService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="Kweet"/> class.
    /// </summary>
    public class Kweet : Entity
    {
        private HashSet<KweetLike> likes;
        private Guid userId;
        private string message;

        /// <summary>
        /// Gets and sets the user id of the kweet.
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
        /// Gets and sets the message of the kweet.
        /// </summary>
        public string Message
        {
            get => message;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new KweetDomainException("The message is null, empty or contains only whitespaces.");
                if (value.Length > 140)
                    throw new KweetDomainException("The length of the message exceeded 140 characters.");
                message = value;
            }
        }

        /// <summary>
        /// Gets and sets the date time when the kweet was created.
        /// </summary>
        public DateTime CreatedDateTime { get; private set; }

        /// <summary>
        /// Gets the like count.
        /// </summary>
        public int LikeCount => likes.Count;

        /// <summary>
        /// Gets a read only set of likes.
        /// </summary>
        public virtual IReadOnlySet<KweetLike> Likes => likes;

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected Kweet() => likes = new HashSet<KweetLike>(new KweetLikeEqualityComparer());

        /// <summary>
        /// Initializes a new instance of the <see cref="Kweet"/> class.
        /// </summary>
        /// <param name="id">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="message">The message.</param>
        internal Kweet(Guid id, Guid userId, string message) : this()
        {
            if (id == Guid.Empty)
                throw new KweetDomainException("The kweet id is empty.");
            Id = id;
            UserId = userId;
            Message = message;
            CreatedDateTime = DateTime.UtcNow;
            AddDomainEvent(new KweetCreatedDomainEvent(Id, UserId, Message, CreatedDateTime));
        }

        /// <summary>
        /// Adds a like to the kweet if the user has not already liked it.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns the like when successfully liking the kweet; otherwise, returns default.</returns>
        /// <exception cref="KweetDomainException">Thrown when the user id is empty.</exception>
        internal KweetLike AddLike(Guid userId)
        {
            KweetLike like = new(Id, userId);
            return likes.Add(like) ? like : default;
        }

        /// <summary>
        /// Removes a like from the kweet if the like of the user is found.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns the removed like when successfully unliking the kweet; otherwise, returns default.</returns>
        /// <exception cref="KweetDomainException">Thrown when the user id is empty.</exception>
        internal KweetLike RemoveLike(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new KweetDomainException("The user id is empty.");
            KweetLike kweetLike = likes.FirstOrDefault(kweet => kweet.UserId == userId);
            if (kweetLike == default)
                return default;
            bool removed = likes.Remove(kweetLike);
            if (removed)
                AddDomainEvent(new KweetUnlikedDomainEvent(userId, Id));
            return kweetLike;
        }

        /// <summary>
        /// Represents the <see cref="KweetLikeEqualityComparer"/> class.
        /// </summary>
        private sealed class KweetLikeEqualityComparer : IEqualityComparer<KweetLike>
        {
            /// <summary>
            /// The like's uniqueness is determined by the user id.
            /// </summary>
            /// <param name="x">Like x to compare.</param>
            /// <param name="y">Like y to compare.</param>
            /// <returns>Returns a boolean to indicate whether the likes are equal.</returns>
            public bool Equals(KweetLike x, KweetLike y) => x.UserId == y.UserId;

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode([DisallowNull] KweetLike obj) => obj.UserId.GetHashCode();
        }
    }
}
