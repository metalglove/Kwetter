using Kwetter.Services.KweetService.Domain.AggregatesModel.KweetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.KweetService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="KweetAggregateConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="KweetService"/> in the DbContext.
    /// </summary>
    internal sealed class KweetAggregateConfiguration : IEntityTypeConfiguration<KweetAggregate>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<KweetAggregate> builder)
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

            // The KweetAggregate owns many KweetLikes.
            builder.OwnsMany<KweetLike>(p => p.Likes, navigationBuilder =>
            {
                // The domain events are not persisted here.
                navigationBuilder.Ignore(p => p.DomainEvents);

                // This is an owned entity type so the id can be ignored.
                navigationBuilder.Ignore(p => p.Id);
            
                // The key is the kweet and user id.
                navigationBuilder.HasKey(p => new {p.KweetId, p.UserId});

                // The liked date time is required.
                navigationBuilder.Property(p => p.LikedDateTime)
                    .IsRequired(true);

                // KweetAggregate is the owner.
                navigationBuilder.WithOwner();
            })
            .Navigation(p => p.Likes)
            .HasField("_likes")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}