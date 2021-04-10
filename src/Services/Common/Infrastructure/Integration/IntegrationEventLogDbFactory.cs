using Kwetter.Services.Common.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventLogDbFactory"/> class.
    /// </summary>
    public sealed class IntegrationEventLogDbFactory : DatabaseFactory<IntegrationEventLogDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventLogDbFactory"/> class.
        /// </summary>
        public IntegrationEventLogDbFactory() : base("EventLog", (IOptions<DbConfiguration>) null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventLogDbFactory"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public IntegrationEventLogDbFactory(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory) : base("EventLog", dbConfigurationOptions, loggerFactory)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventLogDbFactory"/> class.
        /// </summary>
        /// <param name="dbConfiguration">The database configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public IntegrationEventLogDbFactory(DbConfiguration dbConfiguration, ILoggerFactory loggerFactory) : base("EventLog", dbConfiguration, loggerFactory)
        {

        }


        /// <summary>
        /// Creates a new instance of the <see cref="IntegrationEventLogDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>The integration event log database context.</returns>
        protected override IntegrationEventLogDbContext CreateNewInstance(DbContextOptions<IntegrationEventLogDbContext> dbContextOptions)
        {
            return new(dbContextOptions);
        }
    }
}
