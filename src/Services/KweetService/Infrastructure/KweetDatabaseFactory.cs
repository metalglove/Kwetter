using Kwetter.Services.Common.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kwetter.Services.KweetService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="KweetDatabaseFactory"/> class.
    /// </summary>
    public sealed class KweetDatabaseFactory : DatabaseFactory<KweetDbContext>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetDatabaseFactory"/> class.
        /// </summary>
        public KweetDatabaseFactory() : base("Service", (IOptions<DbConfiguration>) default, default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetDatabaseFactory"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        public KweetDatabaseFactory(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory, IMediator mediator) : base("Service", dbConfigurationOptions, loggerFactory)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="KweetDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The kweet database context.</returns>
        protected override KweetDbContext CreateNewInstance(DbContextOptions<KweetDbContext> dbContextOptions)
        {
            return new(dbContextOptions, _mediator);
        }
    }
}