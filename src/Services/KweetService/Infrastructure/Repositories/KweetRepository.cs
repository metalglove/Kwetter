using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.Common.Domain.Persistence;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.Services.KweetService.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the <see cref="KweetRepository"/> class.
    /// </summary>
    public class KweetRepository : IKweetRepository
    {
        private readonly KweetDbContext _kweetDbContext;
        
        /// <inheritdoc cref="IRepository{TAggregate}.UnitOfWork"/>
        public IUnitOfWork UnitOfWork => _kweetDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetRepository"/> class.
        /// </summary>
        /// <param name="kweetDbContext">The kweet database context.</param>
        public KweetRepository(KweetDbContext kweetDbContext)
        {
            _kweetDbContext = kweetDbContext ?? throw new ArgumentNullException(nameof(kweetDbContext));
        }
        
        /// <inheritdoc cref="IKweetRepository.Create(KweetAggregate)" />
        public KweetAggregate Create(KweetAggregate kweet)
        {
            return _kweetDbContext.Kweets
                .Add(kweet).Entity;
        }

        /// <inheritdoc cref="IKweetRepository.FindAsync(Guid, CancellationToken)" />
        public async ValueTask<KweetAggregate> FindAsync(Guid kweetId, CancellationToken cancellationToken)
        {
            return await _kweetDbContext.Kweets.FindAsync(new object[] {kweetId}, cancellationToken);
        }

        /// <inheritdoc cref="IKweetRepository.FindKweetsByUserIdAsync(Guid, CancellationToken)" />
        public async Task<IEnumerable<KweetAggregate>> FindKweetsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _kweetDbContext.Kweets
                .Where(kweet => kweet.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}