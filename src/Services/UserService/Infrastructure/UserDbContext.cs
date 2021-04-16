using Kwetter.Services.Common.Infrastructure;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using Kwetter.Services.UserService.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kwetter.Services.UserService.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="UserDbContext"/> class.
    /// </summary>
    public sealed class UserDbContext : UnitOfWork<UserDbContext>
    {
        /// <summary>
        /// Gets and sets the users database set.
        /// </summary>
        public DbSet<UserAggregate> Users { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDbContext"/> class.
        /// </summary>
        /// <param name="options">The user database context options.</param>
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDbContext"/> class.
        /// </summary>
        /// <param name="options">The user database context options.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public UserDbContext(DbContextOptions<UserDbContext> options, IMediator mediator, ILogger<UserDbContext> logger) : base(options, mediator, logger)
        {

        }

        /// <summary>
        /// Applies the entity configurations during the build of the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAggregateConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
