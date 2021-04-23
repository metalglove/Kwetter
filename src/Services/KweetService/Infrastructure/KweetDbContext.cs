using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;
using Kwetter.Services.KweetService.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kwetter.Services.KweetService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="KweetDbContext"/> class.
    /// </summary>
    public sealed class KweetDbContext : UnitOfWork<KweetDbContext>
    {
        /// <summary>
        /// Gets and sets the kweets database set.
        /// </summary>
        public DbSet<KweetAggregate> Kweets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetDbContext"/> class.
        /// </summary>
        /// <param name="options">The kweet database context options.</param>
        public KweetDbContext(DbContextOptions<KweetDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KweetDbContext"/> class.
        /// </summary>
        /// <param name="options">The kweet database context options.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public KweetDbContext(DbContextOptions<KweetDbContext> options, IMediator mediator, ILogger<KweetDbContext> logger) : base(options, mediator, logger)
        {

        }

        /// <summary>
        /// Applies the entity configurations during the build of the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new KweetAggregateConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}