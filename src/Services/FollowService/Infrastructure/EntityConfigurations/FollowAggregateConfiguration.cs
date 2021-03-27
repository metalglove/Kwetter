using Kwetter.Services.FollowService.Domain.AggregatesModel.FollowAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.FollowService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="FollowAggregateConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="FollowService"/> in the DbContext.
    /// </summary>
    internal sealed class FollowAggregateConfiguration : IEntityTypeConfiguration<FollowAggregate>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<FollowAggregate> builder)
        {
            // The domain events are not persisted here.
            builder.Ignore(p => p.DomainEvents);

            // The default provided Id from the Entity class can be ignored.
            builder.Ignore(p => p.Id);

            // The primary key is the composite key of follower and following id.            
            builder.HasKey(p => new {p.FollowingId, p.FollowerId});

            // The follower id is required.
            builder.Property(p => p.FollowerId)
                .IsRequired(true);

            // The following id is required.
            builder.Property(p => p.FollowingId)
                .IsRequired(true);

            // The created date time is required.
            builder.Property(p => p.FollowDateTime)
                .IsRequired(true);
        }
    }
}