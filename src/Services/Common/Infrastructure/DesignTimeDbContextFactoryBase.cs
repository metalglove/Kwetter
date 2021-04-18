using Kwetter.Services.Common.Application.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
    /// </summary>
    /// <typeparam name="TContext">The database context type.</typeparam>
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private DbContextOptionsBuilder<TContext> _dbContextOptionsBuilder;
        private readonly DbConfiguration _dbConfiguration;
        protected readonly ILoggerFactory LoggerFactory;

        /// <summary>
        /// Initializes an instance of the <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
        /// </summary>
        /// <param name="databaseType">The database type.</param>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        protected DesignTimeDbContextFactoryBase(string databaseType, IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory)
        {
            _dbConfiguration = dbConfigurationOptions == default
                ? GetDbConfiguration(databaseType)
                : dbConfigurationOptions.Value;
            LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
        /// </summary>
        /// <param name="databaseType">The database type.</param>
        /// <param name="dbConfiguration">The database configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        protected DesignTimeDbContextFactoryBase(string databaseType, DbConfiguration dbConfiguration, ILoggerFactory loggerFactory)
        {
            _dbConfiguration = dbConfiguration ?? GetDbConfiguration(databaseType);
            LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// Creates a new DbContext instance using options.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>Returns a new DbContext instance.</returns>
        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> dbContextOptions);

        /// <summary>
        /// Creates a new DbContext instance using string arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Returns a new DbContext instance.</returns>
        public virtual TContext CreateDbContext(string[] args)
        {
            if (_dbContextOptionsBuilder == null)
                SetDbContextOptionsBuilder(_dbConfiguration.ConnectionString, _dbConfiguration.DbProvider);
            return CreateNewInstance(_dbContextOptionsBuilder.Options);
        }

        private void SetDbContextOptionsBuilder(string connectionString, string providerName)
        {
            // If both empty, continue.
            // The in memory database will be selected.
            if (!string.IsNullOrEmpty(providerName) || !string.IsNullOrEmpty(connectionString))
            {
                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentException($"Connection string '{connectionString}' is null or empty.", nameof(connectionString));

                if (string.IsNullOrEmpty(providerName))
                    throw new ArgumentException($"DbProvider string '{providerName}' is null or empty.", nameof(connectionString));
            }

            DbContextOptionsBuilder<TContext> optionsBuilder = new();

            // Adds lazy loading.
            if (_dbConfiguration.UseLazyLoading)
                optionsBuilder.UseLazyLoadingProxies();

            // Switches to the correct DbProvider.
            switch (_dbConfiguration.DbProvider.ToLower())
            {
                case "mssql":
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                case "mssql-in-memory":
                    optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    break;
                case "sqlite":
                    optionsBuilder.UseSqlite(connectionString);
                    break;
                default:
                    optionsBuilder.UseSqlite("DataSource=:memory:");
                    break;
            }

            // Adds logging.
            if (LoggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(LoggerFactory);
                optionsBuilder.EnableSensitiveDataLogging();
            }

            _dbContextOptionsBuilder = optionsBuilder;
        }

        private static DbConfiguration GetDbConfiguration(string databaseType)
        {
            IConfigurationSection configurationSection = databaseType switch
            {
                "Service" => ConfigurationLoader.GetConfiguration("appsettings").GetSection("Database"),
                "EventLog" => ConfigurationLoader.GetConfiguration("appsettings").GetSection("Integration:Database"),
                _ => throw new Exception("Unknown database type passed to database factory.")
            };
            DbConfiguration dbConfiguration = new()
            {
                ConnectionString = configurationSection["ConnectionString"],
                DbProvider = configurationSection["DbProvider"],
                UseLazyLoading = Convert.ToBoolean(configurationSection["UseLazyLoading"])
            };
            return dbConfiguration;
        }
    }
}
