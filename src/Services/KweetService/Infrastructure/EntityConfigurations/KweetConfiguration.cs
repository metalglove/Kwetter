using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.KweetService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="KweetConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="KweetService"/> in the DbContext.
    /// </summary>
    internal sealed class KweetConfiguration : IEntityTypeConfiguration<Kweet>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<Kweet> builder)
        {
            // The domain events are not persisted here.
            builder.Ignore(p => p.DomainEvents);

            // The primary key is Id.
            builder.HasKey(p => p.Id);

            // The user id is required.
            builder.Property(p => p.UserId)
                .IsRequired(true);

            // The created date time is required.
            builder.Property(p => p.CreatedDateTime)
                .IsRequired(true);

            // The message has a maximum length of 140.
            builder.Property(p => p.Message)
                .HasMaxLength(140)
                .IsRequired(true);

            // The kweet has many KweetLikes.
            builder.HasMany(p => p.Likes)
                .WithOne()
                .HasForeignKey(p => p.KweetId);

            builder
                .Navigation(p => p.Likes)
                .HasField("likes")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            // The kweet has many mentions.
            builder.HasMany(p => p.Mentions)
                .WithOne();

            builder
                .Navigation(p => p.Mentions)
                .HasField("mentions")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            // The kweet has many hashtags.
            builder.HasMany(p => p.HashTags)
                .WithOne()
                .HasForeignKey(p => p.KweetId);

            builder
                .Navigation(p => p.HashTags)
                .HasField("hashTags")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            // The like count is computed from the likes set.
            builder.Ignore(p => p.LikeCount);
        }
    }
}
