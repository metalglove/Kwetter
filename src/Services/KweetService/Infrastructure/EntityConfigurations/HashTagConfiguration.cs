using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.KweetService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="HashTagConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="KweetService"/> in the DbContext.
    /// </summary>
    internal sealed class HashTagConfiguration : IEntityTypeConfiguration<HashTag>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<HashTag> builder)
        {
            // The domain events are not persisted here.
            builder.Ignore(p => p.DomainEvents);

            // The id can be ignored.
            builder.Ignore(p => p.Id);

            // The key is the kweet and hash tag.
            builder.HasKey(p => new { p.KweetId, p.Tag });
        }
    }
}
