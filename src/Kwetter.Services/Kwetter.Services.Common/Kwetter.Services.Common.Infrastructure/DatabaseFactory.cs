using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="DatabaseFactory{TDbContext}"/> class; an implementation of the abstract <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
    /// </summary>
    public abstract class DatabaseFactory<TDbContext> : DesignTimeDbContextFactoryBase<TDbContext>, IFactory<TDbContext> where TDbContext : DbContext
    {
        private readonly string[] _arguments = { "" };

        /// <summary>
        /// Initializes in instance of the <see cref="DatabaseFactory{TDbContext}"/> class.
        /// </summary>
        protected DatabaseFactory() : base("Service", (IOptions<DbConfiguration>) null, null)
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="DatabaseFactory{TDbContext}"/> class.
        /// </summary>
        /// <param name="databaseType">The database type.</param>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        protected DatabaseFactory(string databaseType, IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory) : base(databaseType, dbConfigurationOptions, loggerFactory)
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="DatabaseFactory{TDbContext}"/> class.
        /// </summary>
        /// <param name="databaseType">The database type.</param>
        /// <param name="dbConfiguration">The database configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        protected DatabaseFactory(string databaseType, DbConfiguration dbConfiguration, ILoggerFactory loggerFactory) : base(databaseType, dbConfiguration, loggerFactory)
        {

        }

        /// <inheritdoc cref="IFactory{TResult}.Create"/>
        public TDbContext Create()
        {
            return base.CreateDbContext(_arguments);
        }
    }
}
