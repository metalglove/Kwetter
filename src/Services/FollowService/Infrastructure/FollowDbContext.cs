using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.FollowService.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kwetter.Services.FollowService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="FollowDbContext"/> class.
    /// </summary>
    public sealed class FollowDbContext : UnitOfWork<FollowDbContext>
    {
        /// <summary>
        /// Gets and sets the users database set.
        /// </summary>
        public DbSet<UserAggregate> Users { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowDbContext"/> class.
        /// </summary>
        /// <param name="options">The follow database context options.</param>
        public FollowDbContext(DbContextOptions<FollowDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowDbContext"/> class.
        /// </summary>
        /// <param name="options">The follow database context options.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public FollowDbContext(DbContextOptions<FollowDbContext> options, IMediator mediator, ILogger<FollowDbContext> logger) : base(options, mediator, logger)
        {

        }

        /// <summary>
        /// Applies the entity configurations during the build of the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAggregateConfiguration());
            modelBuilder.ApplyConfiguration(new FollowConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}