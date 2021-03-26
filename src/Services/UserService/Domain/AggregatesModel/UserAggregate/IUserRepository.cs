using Kwetter.Services.Common.Domain.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate
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
        /// Updates the user aggregate.
        /// </summary>
        /// <param name="user">The user aggregate.</param>
        /// <returns>Returns the updated user aggregate.</returns>
        UserAggregate Update(UserAggregate user);

        /// <summary>
        /// Finds the user aggregate by id asynchronously.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the user aggregate.</returns>
        Task<UserAggregate> FindByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the user aggregate by username asynchronously.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the user aggregate.</returns>
        Task<UserAggregate> FindByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
