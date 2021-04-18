using Kwetter.Services.Common.Application.Configurations;
using Kwetter.Services.Common.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kwetter.Services.FollowService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="FollowDatabaseFactory"/> class.
    /// </summary>
    public sealed class FollowDatabaseFactory : DatabaseFactory<FollowDbContext>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowDatabaseFactory"/> class.
        /// </summary>
        public FollowDatabaseFactory() : base("Service", (IOptions<DbConfiguration>) default, default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowDatabaseFactory"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        public FollowDatabaseFactory(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory, IMediator mediator) : base("Service", dbConfigurationOptions, loggerFactory)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FollowDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The follow database context.</returns>
        protected override FollowDbContext CreateNewInstance(DbContextOptions<FollowDbContext> dbContextOptions)
        {
            return new(dbContextOptions, _mediator, LoggerFactory.CreateLogger<FollowDbContext>());
        }
    }
}