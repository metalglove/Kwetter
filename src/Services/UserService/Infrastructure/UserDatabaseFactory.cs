using Kwetter.Services.Common.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kwetter.Services.UserService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="UserDatabaseFactory"/> class.
    /// </summary>
    public sealed class UserDatabaseFactory : DatabaseFactory<UserDbContext>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatabaseFactory"/> class.
        /// </summary>
        public UserDatabaseFactory() : base("Service", (IOptions<DbConfiguration>) null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatabaseFactory"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        public UserDatabaseFactory(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory, IMediator mediator) : base("Service", dbConfigurationOptions, loggerFactory)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UserDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The user database context.</returns>
        protected override UserDbContext CreateNewInstance(DbContextOptions<UserDbContext> dbContextOptions)
        {
            return new(dbContextOptions, _mediator);
        }
    }
}
