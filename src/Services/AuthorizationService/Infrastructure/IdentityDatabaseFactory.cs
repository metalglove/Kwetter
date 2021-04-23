using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kwetter.Services.AuthorizationService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="IdentityDatabaseFactory"/> class.
    /// </summary>
    public sealed class IdentityDatabaseFactory : DatabaseFactory<IdentityDbContext>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDatabaseFactory"/> class.
        /// </summary>
        public IdentityDatabaseFactory() : base("Service", (IOptions<DbConfiguration>)default, default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDatabaseFactory"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        public IdentityDatabaseFactory(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory, IMediator mediator) : base("Service", dbConfigurationOptions, loggerFactory)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IdentityDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The identity database context.</returns>
        protected override IdentityDbContext CreateNewInstance(DbContextOptions<IdentityDbContext> dbContextOptions)
        {
            return LoggerFactory is null
                ? (new(dbContextOptions, _mediator, default))
                : (new(dbContextOptions, _mediator, LoggerFactory.CreateLogger<IdentityDbContext>()));
        }
    }
}
