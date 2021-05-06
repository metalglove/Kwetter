using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.KweetService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="KweetLikeConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="KweetService"/> in the DbContext.
    /// </summary>
    internal sealed class KweetLikeConfiguration : IEntityTypeConfiguration<KweetLike>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<KweetLike> builder)
        {
            // The domain events are not persisted here.
            builder.Ignore(p => p.DomainEvents);

            // This is an owned entity type so the id can be ignored.
            builder.Ignore(p => p.Id);

            // The key is the kweet and user id.
            builder.HasKey(p => new { p.KweetId, p.UserId });

            // The liked date time is required.
            builder.Property(p => p.LikedDateTime)
                .IsRequired(true);
        }
    }
}
