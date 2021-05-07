using Kwetter.Services.Common.Domain.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate
{
    /// <summary>
    /// Represents the <see cref="IIdentityRepository"/> interface.
    /// </summary>
    public interface IIdentityRepository : IRepository<IdentityAggregate>
    {
        /// <summary>
        /// Creates the identity.
        /// </summary>
        /// <param name="identity">The identity aggregate.</param>
        /// <returns>Returns the tracked identity aggregate.</returns>
        IdentityAggregate Create(IdentityAggregate identity);

        /// <summary>
        /// Finds the identity aggregate with the given id asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the identity aggregate if found; otherwise, default.</returns>
        ValueTask<IdentityAggregate> FindAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the identity aggregate with the given user name asynchronously.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the identity with the given user name.</returns>
        Task<IdentityAggregate> FindByUserNameAsync(string userName, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the identity aggregate with the given open id asynchronously.
        /// </summary>
        /// <param name="openId">The open id.</param>
        /// <param name="cancellationToken">The cancellation token/</param>
        /// <returns>Returns the identity aggregate if found; otherwise, default.</returns>
        Task<IdentityAggregate> FindIdentityByOpenIdAsync(string openId, CancellationToken cancellationToken);
    }
}
