using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kwetter.Services.Common.Domain.Persistence;
using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.Services.FollowService.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the <see cref="FollowRepository"/> class.
    /// </summary>
    public class FollowRepository : IFollowRepository
    {
        private readonly FollowDbContext _followDbContext;

        /// <inheritdoc cref="IRepository{TAggregate}.UnitOfWork"/>
        public IUnitOfWork UnitOfWork => _followDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowRepository"/> class.
        /// </summary>
        /// <param name="followDbContext">The follow database context.</param>
        public FollowRepository(FollowDbContext followDbContext)
        {
            _followDbContext = followDbContext ?? throw new ArgumentNullException(nameof(followDbContext));
        }
        
        /// <inheritdoc cref="IFollowRepository.Create(FollowAggregate)" />
        public FollowAggregate Create(FollowAggregate follow)
        {
            return _followDbContext.Follows
                .Add(follow).Entity;
        }

        /// <inheritdoc cref="IFollowRepository.Delete(FollowAggregate)" />
        public void Delete(FollowAggregate follow)
        {
            _followDbContext.Follows.Remove(follow);
        }

        /// <inheritdoc cref="IFollowRepository.FindAsync" />
        public async ValueTask<FollowAggregate> FindAsync(Guid followingId, Guid followerId, CancellationToken cancellationToken)
        {
            return await _followDbContext.Follows.FindAsync(new object[] {followingId, followerId}, cancellationToken);
        }

        /// <inheritdoc cref="IFollowRepository.FindFollowersByUserIdAsync(Guid, CancellationToken)" />
        public async Task<IEnumerable<FollowAggregate>> FindFollowersByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _followDbContext.Follows
                .Where(follow => follow.FollowingId == userId)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc cref="IFollowRepository.FindFollowingsByUserIdAsync(Guid, CancellationToken)" />
        public async Task<IEnumerable<FollowAggregate>> FindFollowingsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _followDbContext.Follows
                .Where(follow => follow.FollowerId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}