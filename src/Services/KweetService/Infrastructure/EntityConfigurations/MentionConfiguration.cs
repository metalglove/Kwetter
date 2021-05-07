using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.KweetService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="MentionConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="KweetService"/> in the DbContext.
    /// </summary>
    internal sealed class MentionConfiguration : IEntityTypeConfiguration<Mention>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Mention> builder)
        {
            // The domain events are not persisted here.
            builder.Ignore(p => p.DomainEvents);

            // The id can be ignored.
            builder.Ignore(p => p.Id);

            // The key is the kweet and user name.
            builder.HasKey(p => new { p.KweetId, p.UserName });

            builder.HasOne(p => p.User)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
