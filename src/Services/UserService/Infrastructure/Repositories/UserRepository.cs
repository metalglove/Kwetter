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
            //if (user.IsTransient())
                return _userDbContext.Users
                    .Add(user).Entity;
            //return user;
        }

        /// <inheritdoc cref="IUserRepository.Update(UserAggregate)" />
        public UserAggregate Update(UserAggregate user)
        {
            return _userDbContext.Update(user).Entity;
        }

        /// <inheritdoc cref="IUserRepository.FindByIdAsync(Guid)" />
        public async Task<UserAggregate> FindByIdAsync(Guid id)
        {
            return await _userDbContext.Users
                .FindAsync(id);
        }

        /// <inheritdoc cref="IUserRepository.FindByUsernameAsync(string, CancellationToken)" />
        public async Task<UserAggregate> FindByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _userDbContext.Users
                .Where(user => user.Username.ToLower() == username.ToLower())
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
