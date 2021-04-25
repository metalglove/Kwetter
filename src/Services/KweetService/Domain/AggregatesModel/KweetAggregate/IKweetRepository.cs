using Kwetter.Services.Common.Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate
{
    /// <summary>
    /// Represents the <see cref="IKweetRepository"/> interface.
    /// </summary>
    public interface IKweetRepository : IRepository<KweetAggregate>
    {
        /// <summary>
        /// Creates the kweet aggregate.
        /// </summary>
        /// <param name="kweet">The kweet aggregate.</param>
        /// <returns>Returns the tracked kweet aggregate.</returns>
        KweetAggregate Create(KweetAggregate kweet);
        
        /// <summary>
        /// Finds the kweet aggregate with the given id asynchronously.
        /// </summary>
        /// <param name="kweetId">The kweet id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the kweet aggregate if found; otherwise, default.</returns>
        ValueTask<KweetAggregate> FindAsync(Guid kweetId,  CancellationToken cancellationToken);

        /// <summary>
        /// Finds kweets by the user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token/</param>
        /// <returns>Returns a collection of kweets for the provided user id.</returns>
        Task<IEnumerable<KweetAggregate>> FindKweetsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
