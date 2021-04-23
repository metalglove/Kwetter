using Kwetter.Services.Common.Domain.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UnitOfWork<TContext>> _logger;

        /// <summary>
        /// Gets and sets the current transaction.
        /// </summary>
        protected IDbContextTransaction CurrentTransaction { get; private set; }

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
        /// <param name="logger">The logger.</param>
        protected UnitOfWork(DbContextOptions<TContext> options, IMediator mediator, ILogger<UnitOfWork<TContext>> logger) : base(options)
        {
            _mediator = mediator;
            _logger = logger;
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
            await _mediator.DispatchDomainEventsAsync(this, cancellationToken);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <inheritdoc cref="IAggregateUnitOfWork.BeginTransactionAsync(CancellationToken)"/>
        public async Task<Guid> StartTransactionAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Check whether this is good... should be scoped service lifetime, but still..
            if (CurrentTransaction != null) 
                return CurrentTransaction.TransactionId;

            CurrentTransaction = await Database.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation($"Started the database transaction {CurrentTransaction.TransactionId} for {GetType().Name}");
            return CurrentTransaction.TransactionId;
        }

        /// <inheritdoc cref="IAggregateUnitOfWork.CommitTransactionAsync(CancellationToken)"/>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (CurrentTransaction is null)
                throw new InvalidOperationException($"Transaction is null.");
            try
            {
                await SaveChangesAsync(cancellationToken);
                await CurrentTransaction.CommitAsync(cancellationToken);
                _logger.LogInformation($"Commited the database transaction {CurrentTransaction.TransactionId} for {GetType().Name}");

            }
            catch
            {
                await RollbackTransactionAsync(default);
                throw;
            }
            finally
            {
                CurrentTransaction?.Dispose();
                CurrentTransaction = null;
            }
        }

        /// <inheritdoc cref="IAggregateUnitOfWork.RollbackTransactionAsync(CancellationToken)"/>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await CurrentTransaction?.RollbackAsync(cancellationToken);
                _logger.LogInformation($"Rolled back the database transaction {CurrentTransaction.TransactionId} for {GetType().Name}");
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
