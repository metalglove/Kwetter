using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Kwetter.Services.AuthorizationService.Infrastructure.EntityConfigurations;
using Kwetter.Services.Common.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kwetter.Services.AuthorizationService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="IdentityDbContext"/> class.
    /// </summary>
    public sealed class IdentityDbContext : UnitOfWork<IdentityDbContext>
    {
        /// <summary>
        /// Gets and sets the identities database set.
        /// </summary>
        public DbSet<IdentityAggregate> Identities { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDbContext"/> class.
        /// </summary>
        /// <param name="options">The identity database context options.</param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDbContext"/> class.
        /// </summary>
        /// <param name="options">The identity database context options.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IMediator mediator, ILogger<IdentityDbContext> logger) : base(options, mediator, logger)
        {
            
        }

        /// <summary>
        /// Applies the entity configurations during the build of the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IdentityAggregateConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
