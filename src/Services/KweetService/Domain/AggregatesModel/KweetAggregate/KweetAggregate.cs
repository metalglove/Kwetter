using Kwetter.Services.Common.Domain;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate.Events;
using Kwetter.Services.KweetService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate
{
    /// <summary>
    /// Represents the <see cref="KweetAggregate"/> class.
    /// The kweet aggregate is the aggregate root for the KweetService.
    /// </summary>
    public class KweetAggregate : Entity, IAggregateRoot
    {
        private HashSet<KweetLike> _likes;

        /// <summary>
        /// Gets and sets the user id of the kweet.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets and sets the message of the kweet.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets and sets the date time when the kweet was created.
        /// </summary>
        public DateTime CreatedDateTime { get; private set; }

        /// <summary>
        /// Gets the like count.
        /// </summary>
        public int LikeCount => _likes.Count;

        /// <summary>
        /// Gets and sets the likes of the kweet.
        /// </summary>
        public IReadOnlySet<KweetLike> Likes => _likes;

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected KweetAggregate() => _likes = new HashSet<KweetLike>(new KweetLikeEqualityComparer());

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetAggregate"/> class.
        /// </summary>
        /// <param name="id">The kweet id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="message">The message.</param>
        public KweetAggregate(Guid id, Guid userId, string message)
        {
            SetId(id);
            SetUserId(userId);
            SetMessage(message);
            CreatedDateTime = DateTime.UtcNow;
            _likes = new HashSet<KweetLike>(new KweetLikeEqualityComparer());
            AddDomainEvent(new KweetCreatedDomainEvent(Id, UserId, Message, CreatedDateTime));
        }

        /// <summary>
        /// Adds a like to the kweet if the user has not already liked it.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns a boolean indicating whether a like was added.</returns>
        /// <exception cref="KweetDomainException">Thrown when the user id is empty.</exception>
        public bool AddLike(Guid userId)
        {
            KweetLike like = new(Id, userId);
            return _likes.Add(like);
        }

        /// <summary>
        /// Removes a like from the kweet if the like of the user is found.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns a boolean indicating whether a like was removed.</returns>
        /// <exception cref="KweetDomainException">Thrown when the user id is empty.</exception>
        public bool RemoveLike(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new KweetDomainException("The user id is empty.");
            bool removed = _likes.RemoveWhere(kweet => kweet.UserId == userId) == 1;
            if (removed)
                AddDomainEvent(new KweetUnlikedDomainEvent(userId, Id));
            return removed;
        }

        /// <summary>
        /// Sets the id of the kweet.
        /// </summary>
        /// <param name="id">The kweet id.</param>
        /// <exception cref="KweetDomainException">Thrown when the provided kweet id is empty or malformed.</exception>
        private void SetId(Guid id)
        {
            if (id == Guid.Empty)
                throw new KweetDomainException("The kweet id is empty.");
            Id = id;
        }

        /// <summary>
        /// Sets the user id of the kweet.
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
        /// Sets the message of the kweet.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="KweetDomainException">Thrown when the provided message exceeds 140 characters or is empty.</exception>
        private void SetMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new KweetDomainException("The message is null, empty or contains only whitespaces.");
            if (message.Length > 140)
                throw new KweetDomainException("The length of the message exceeded 140 characters.");
            Message = message;
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
