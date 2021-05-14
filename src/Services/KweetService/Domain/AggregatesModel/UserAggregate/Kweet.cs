using Kwetter.Services.Common.Domain;
using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate.Events;
using Kwetter.Services.KweetService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="Kweet"/> class.
    /// </summary>
    public class Kweet : Entity
    {
        private HashSet<KweetLike> likes;
        private HashSet<HashTag> hashTags;
        private HashSet<Mention> mentions;
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

                Regex mentionRegex = new(@"(?<=^|(?<=[^a-zA-Z0-9\.]))@([A-Za-z0-9]{3,32})", RegexOptions.Compiled);
                foreach (Match match in mentionRegex.Matches(value))
                {
                    mentions.Add(new Mention(match.Value.ToLowerInvariant().Remove(0, 1), this));
                }

                Regex hashTagRegex = new(@"(?<=^|(?<=[^a-zA-Z0-9\.]))#([A-Za-z0-9]{3,32})", RegexOptions.Compiled);
                foreach (Match match in hashTagRegex.Matches(value))
                {
                    hashTags.Add(new HashTag(match.Value.ToLowerInvariant(), Id));
                }

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
        /// Gets a readonly set of hashtags.
        /// </summary>
        public virtual IReadOnlySet<HashTag> HashTags => hashTags;

        /// <summary>
        /// Gets a readonly set of mentions.
        /// </summary>
        public virtual IReadOnlySet<Mention> Mentions => mentions;

        /// <summary>
        /// EF constructor...
        /// </summary>
        protected Kweet()
        {
            likes = new HashSet<KweetLike>(new KweetLikeEqualityComparer());
            hashTags = new HashSet<HashTag>(new HashTagEqualityComparer());
            mentions = new HashSet<Mention>(new MentionEqualityComparer());
        }

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
            AddDomainEvent(new KweetCreatedDomainEvent(Id, UserId, Message, ref hashTags, ref mentions, CreatedDateTime));
        }

        /// <summary>
        /// Processes the mentions found in the message.
        /// </summary>
        /// <param name="findUsersByUserNamesAsync">The find users by user names helper task.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        internal async ValueTask ProcessMentionsAsync(Func<IEnumerable<Mention>, CancellationToken, Task<IEnumerable<Mention>>> findUsersByUserNamesAsync, CancellationToken cancellationToken)
        {
            if (mentions.Any())
            {
                Task<IEnumerable<Mention>> task = findUsersByUserNamesAsync(mentions, cancellationToken);
                mentions.Clear();
                foreach (Mention mention in await task)
                {
                    mentions.Add(mention);
                }
            }
        }

        /// <summary>
        /// Adds a like to the kweet if the user has not already liked it.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns the like when successfully liking the kweet; otherwise, returns default.</returns>
        /// <exception cref="KweetDomainException">Thrown when the user id is empty.</exception>
        internal KweetLike AddLike(Guid userId)
        {
            KweetLike like = new(this, userId);
            if (Likes.Contains(like))
                return default;
            likes.Add(like);
            return like;
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
            KweetLike kweetLike = Likes.FirstOrDefault(like => like.UserId == userId);
            if (kweetLike == default)
                return default;
            bool removed = likes.Remove(kweetLike);
            if (removed)
                AddDomainEvent(new KweetUnlikedDomainEvent(Id, userId));
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

        /// <summary>
        /// Represents the <see cref="HashTagEqualityComparer"/> class.
        /// </summary>
        private sealed class HashTagEqualityComparer : IEqualityComparer<HashTag>
        {
            /// <summary>
            /// The hash tag's uniqeness is determined by the tag.
            /// </summary>
            /// <param name="x">Hash tag x to compare.</param>
            /// <param name="y">Hash tag y to compare.</param>
            /// <returns>Returns a boolean to indicate whether the hash tags are equal.</returns>
            public bool Equals(HashTag x, HashTag y) => x.Tag == y.Tag;

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode([DisallowNull] HashTag obj) => obj.Tag.GetHashCode();
        }

        /// <summary>
        /// Represents the <see cref="MentionEqualityComparer"/> class.
        /// </summary>
        private sealed class MentionEqualityComparer : IEqualityComparer<Mention>
        {
            /// <summary>
            /// The mention's uniqeness is determined by the username.
            /// </summary>
            /// <param name="x">Mention x to compare.</param>
            /// <param name="y">Mention y to compare.</param>
            /// <returns>Returns a boolean to indicate whether the mentions are equal.</returns>
            public bool Equals(Mention x, Mention y) => x.UserName == y.UserName;

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode([DisallowNull] Mention obj) => obj.UserName.GetHashCode();
        }
    }
}
