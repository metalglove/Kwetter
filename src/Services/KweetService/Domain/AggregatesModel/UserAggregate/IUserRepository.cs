using Kwetter.Services.Common.Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Represents the <see cref="IUserRepository"/> interface.
    /// </summary>
    public interface IUserRepository : IRepository<UserAggregate>
    {
        /// <summary>
        /// Creates the user aggregate.
        /// </summary>
        /// <param name="user">The user aggregate.</param>
        /// <returns>Returns the tracked user aggregate.</returns>
        UserAggregate Create(UserAggregate user);

        /// <summary>
        /// Tracks the kweet.
        /// </summary>
        /// <param name="kweet">The kweet.</param>
        /// <returns>Returns the tracked kweet.</returns>
        Kweet TrackKweet(Kweet kweet);

        /// <summary>
        /// Finds the user by id asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the user aggregate if found; otherwise, default.</returns>
        ValueTask<UserAggregate> FindAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the kweet with the given kweet id asynchronously.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the kweet if found; otherwise, default.</returns>
        ValueTask<Kweet> FindKweetAsync(Guid kweetId, CancellationToken cancellationToken);

        /// <summary>
        /// Find and track mentions asynchronously.
        /// </summary>
        /// <param name="mentions">The mentions.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the tracked mentions.</returns>
        Task<IEnumerable<Mention>> FindUsersByUserNameAndTrackMentionsAsync(IEnumerable<Mention> mentions, CancellationToken cancellationToken);
    }
}
