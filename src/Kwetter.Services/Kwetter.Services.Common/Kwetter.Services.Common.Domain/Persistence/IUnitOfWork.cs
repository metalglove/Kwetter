using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Domain.Persistence
{
    /// <summary>
    /// Represents the <see cref="IUnitOfWork"/> interface.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves the changes to the unit of work asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an integer indicating the changed records.</returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the entities to the unit of work asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns a bool indicating whether the entities where saved.</returns>
        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}