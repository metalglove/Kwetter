using Microsoft.EntityFrameworkCore;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventLogDbContext"/> class.
    /// NOTE:
    ///     For some reason, the context does not get picked up when performing a migration in the command line.
    ///     Use this work around:
    ///         dotnet-ef migrations add "InitialCreate" -c IntegrationEventLogDbContext --configuration .\appsettings.json
    ///         dotnet-ef database update -c IntegrationEventLogDbContext --configuration .\appsettings.json
    /// </summary>
    public sealed class IntegrationEventLogDbContext : DbContext
    {
        /// <summary>
        /// Gets and sets the integration event log entries.
        /// </summary>
        public DbSet<IntegrationEventLogEntry> IntegrationEventLogEntries { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventLogDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public IntegrationEventLogDbContext(DbContextOptions<IntegrationEventLogDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Applies the configurations to the database context.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IntegrationEventLogConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
