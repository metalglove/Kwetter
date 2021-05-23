using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="DatabaseChecker"/> class.
    /// </summary>
    public static class DatabaseChecker
    {
        /// <summary>
        /// Verifies the database connection (with EF CORE).
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <typeparam name="TDbContext">The type of database.</typeparam>
        /// <exception cref="TargetException">Thrown when a non-relational database provider is used.</exception>
        public static void VerifyDatabaseConnection<TDbContext>(this IServiceCollection serviceCollection) where TDbContext : DbContext //UnitOfWork<TDbContext>
        {
            ServiceProvider buildServiceProvider = serviceCollection.BuildServiceProvider();
            ILogger logger = buildServiceProvider.GetRequiredService<ILogger<TDbContext>>();
            TDbContext dbContext = buildServiceProvider.GetRequiredService<TDbContext>();
            try
            {
                logger.LogInformation("Starting database verification..");
                if (dbContext.Database.IsInMemory())
                {
                    logger.LogInformation("InMemory database provider found.");
                    dbContext.Database.EnsureCreated();
                    logger.LogInformation("Ensured that the database is created.");
                    return;
                }

                if (dbContext.Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator relationalDatabaseCreator)
                    throw new TargetException("The DbProvider is not relational database provider.");

                bool exists = relationalDatabaseCreator.Exists();
                logger.LogInformation($"Database {(exists ? "" : "does not ")}exists.");
                if (!exists)
                {
                    relationalDatabaseCreator.Create();
                    logger.LogInformation("Database created.");
                }

                IEnumerable<string> pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();
                bool hasPendingMigrations = pendingMigrations.Any();
                logger.LogInformation($"Database {(hasPendingMigrations ? "has" : "does not have")} pending migrations.");
                if (hasPendingMigrations)
                {
                    logger.LogInformation("Ensuring that the Database is deleted to reapply migrations. ");
                    dbContext.Database.EnsureDeleted();
                    foreach (string migration in dbContext.Database.GetMigrations())
                    {
                        logger.LogInformation($"\tMigration: {migration}");
                    }
                    dbContext.Database.Migrate();
                    logger.LogInformation("Applied the migrations to the database.");
                }

                logger.LogInformation("Finished database verification!");
            }
            catch (Exception e)
            {
                logger.LogInformation($"VerifyDatabaseConnection is cancelled. {e.Message}");
                throw;
            }
            finally
            {
                dbContext.Dispose();
            }
        }
    }
}
