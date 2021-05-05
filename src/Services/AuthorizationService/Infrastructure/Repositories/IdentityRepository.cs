using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.Common.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Services.AuthorizationService.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the <see cref="IdentityRepository"/> class.
    /// </summary>
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IdentityDbContext _identityDbContext;

        /// <inheritdoc cref="IRepository{TAggregate}.UnitOfWork"/>
        public IUnitOfWork UnitOfWork => _identityDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRepository"/> class.
        /// </summary>
        /// <param name="identityDbContext">The identity database context.</param>
        public IdentityRepository(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        /// <inheritdoc cref="IIdentityRepository.Create(IdentityAggregate)"/>
        public IdentityAggregate Create(IdentityAggregate identity)
        {
            return _identityDbContext.Identities
                .Add(identity).Entity;
        }

        /// <inheritdoc cref="IIdentityRepository.FindAsync(Guid, CancellationToken)"/>
        public async ValueTask<IdentityAggregate> FindAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _identityDbContext.Identities
                .FindAsync(new object[] { userId }, cancellationToken);
        }

        /// <inheritdoc cref="IIdentityRepository.FindIdentityByOpenIdAsync(string, CancellationToken)"/>
        public async Task<IdentityAggregate> FindIdentityByOpenIdAsync(string openId, CancellationToken cancellationToken)
        {
            return await _identityDbContext.Identities
                .AsQueryable()
                .SingleOrDefaultAsync(identity => identity.OpenId == openId, cancellationToken);
        }

        /// <inheritdoc cref="IIdentityRepository.FindByUserNameAsync(string, CancellationToken)"/>
        public async Task<IdentityAggregate> FindByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            string loweredUserName = userName.ToLower();
            return await _identityDbContext.Identities
                .AsQueryable()
                .SingleOrDefaultAsync(identity => identity.UserName == loweredUserName, cancellationToken);
        }
    }
}
