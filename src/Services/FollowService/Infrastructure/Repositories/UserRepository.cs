using Kwetter.Services.Common.Domain.Persistence;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.FollowService.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the <see cref="UserRepository"/> class.
    /// </summary>
    public sealed class UserRepository : IUserRepository
    {
        private readonly FollowDbContext _followDbContext;

        /// <inheritdoc cref="IRepository{TAggregate}.UnitOfWork"/>
        public IUnitOfWork UnitOfWork => _followDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="followDbContext">The follow database context.</param>
        public UserRepository(FollowDbContext followDbContext)
        {
            _followDbContext = followDbContext ?? throw new ArgumentNullException(nameof(followDbContext));
        }

        /// <inheritdoc cref="IUserRepository.Create(UserAggregate)" />
        public UserAggregate Create(UserAggregate user)
        {
            return _followDbContext.Users
                .Add(user).Entity;
        }

        /// <inheritdoc cref="IUserRepository.FindAsync(Guid, CancellationToken)" />
        public async ValueTask<UserAggregate> FindAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _followDbContext.Users
                .FindAsync(new object[] { userId }, cancellationToken);
        }
    }
}
