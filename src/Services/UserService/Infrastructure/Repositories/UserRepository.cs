using Kwetter.Services.Common.Domain.Persistence;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.UserService.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the <see cref="UserRepository"/> class.
    /// </summary>
    public sealed class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;

        /// <inheritdoc cref="IRepository{TAggregate}.UnitOfWork"/>
        public IUnitOfWork UnitOfWork => _userDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="userDbContext">The user database context.</param>
        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        }

        /// <inheritdoc cref="IUserRepository.Create(UserAggregate)" />
        public UserAggregate Create(UserAggregate user)
        {
            return _userDbContext.Users
                .Add(user).Entity;
        }

        /// <inheritdoc cref="IUserRepository.Update(UserAggregate)" />
        public UserAggregate Update(UserAggregate user)
        {
            return _userDbContext.Update(user).Entity;
        }

        /// <inheritdoc cref="IUserRepository.FindByIdAsync(Guid, CancellationToken)" />
        public async Task<UserAggregate> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _userDbContext.Users
                .FindAsync(new object[] {id}, cancellationToken);
        }

        /// <inheritdoc cref="IUserRepository.FindByUserDisplayNameAsync" />
        public async Task<UserAggregate> FindByUserDisplayNameAsync(string displayName, CancellationToken cancellationToken)
        {
            return await _userDbContext.Users
                .AsQueryable().Where(user => user.DisplayName.ToLower() == displayName.ToLower())
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
