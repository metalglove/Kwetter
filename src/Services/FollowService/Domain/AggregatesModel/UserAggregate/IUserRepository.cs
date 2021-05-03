using Kwetter.Services.Common.Domain.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate
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
        /// Finds the user by id asynchronously.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the user aggregate if found; otherwise, default.</returns>
        ValueTask<UserAggregate> FindAsync(Guid userId, CancellationToken cancellationToken);
    }
}
