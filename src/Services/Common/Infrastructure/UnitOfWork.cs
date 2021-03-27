using Kwetter.Services.Common.Domain.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="UnitOfWork{TContext}"/> class.
    /// Holds the base implementation for the unit of work pattern.
    /// </summary>
    public abstract class UnitOfWork<TContext> : DbContext, IAggregateUnitOfWork where TContext : DbContext
    {
        private readonly IMediator _mediator;

        /// <inheritdoc cref="IAggregateUnitOfWork.CurrentTransaction"/>
        public IDbContextTransaction CurrentTransaction { get; private set; }

        /// <inheritdoc cref="IAggregateUnitOfWork.HasActiveTransaction"/>
        public bool HasActiveTransaction => CurrentTransaction != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{Context}"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        protected UnitOfWork(DbContextOptions<TContext> options) : base(options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{Context}"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        /// <param name="mediator">The mediator.</param>
        protected UnitOfWork(DbContextOptions<TContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        /// <inheritdoc cref="IUnitOfWork.SaveEntitiesAsync(CancellationToken)"/>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            //    side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <inheritdoc cref="IAggregateUnitOfWork.BeginTransactionAsync(CancellationToken)"/>
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (CurrentTransaction != null) 
                return null;
            CurrentTransaction = await Database.BeginTransactionAsync(cancellationToken);
            return CurrentTransaction;
        }

        /// <inheritdoc cref="IAggregateUnitOfWork.CommitTransactionAsync(IDbContextTransaction)"/>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            if (transaction != CurrentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        /// <inheritdoc cref="IAggregateUnitOfWork.RollbackTransaction()"/>
        public void RollbackTransaction()
        {
            try
            {
                CurrentTransaction?.Rollback();
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }
    }
}
