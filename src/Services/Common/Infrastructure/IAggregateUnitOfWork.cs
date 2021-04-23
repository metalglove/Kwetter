using Kwetter.Services.Common.Domain.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="IAggregateUnitOfWork"/> interface.
    /// </summary>
    public interface IAggregateUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets the database facade.
        /// </summary>
        public DatabaseFacade Database { get; }

        /// <summary>
        /// Starts the database transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable database transaction.</returns>
        public Task<Guid> StartTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the transaction to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the database transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable task.</returns>
        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
