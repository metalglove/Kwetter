using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.Common.Domain.Persistence;

namespace Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate
{
    /// <summary>
    /// Represents the <see cref="IFollowRepository"/> interface.
    /// </summary>
    public interface IFollowRepository : IRepository<FollowAggregate>
    {
        /// <summary>
        /// Creates the follow aggregate.
        /// </summary>
        /// <param name="follow">The follow aggregate.</param>
        /// <returns>Returns the tracked follow aggregate.</returns>
        FollowAggregate Create(FollowAggregate follow);
        
        /// <summary>
        /// Deletes the follow aggregate.
        /// </summary>
        /// <param name="follow">The follow aggregate.</param>
        void Delete(FollowAggregate follow);

        /// <summary>
        /// Finds the follow aggregate with the given composite key asynchronously.
        /// </summary>
        /// <param name="followingId">The following id.</param>
        /// <param name="followerId">The follower id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the follow aggregate if found; otherwise, default.</returns>
        ValueTask<FollowAggregate> FindAsync(Guid followingId, Guid followerId, CancellationToken cancellationToken);
        
        // TODO: Add pagination issue
        
        /// <summary>
        /// Finds followers by the user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token/</param>
        /// <returns>Returns a collection of followers for the provided user id.</returns>
        Task<IEnumerable<FollowAggregate>> FindFollowersByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Finds followings by the user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token/</param>
        /// <returns>Returns a collection of followings for the provided user id.</returns>
        Task<IEnumerable<FollowAggregate>> FindFollowingsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}