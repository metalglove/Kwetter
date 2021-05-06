using Kwetter.Services.KweetService.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.KweetService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="UserAggregateConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="KweetService"/> in the DbContext.
    /// </summary>
    internal sealed class UserAggregateConfiguration : IEntityTypeConfiguration<UserAggregate>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<UserAggregate> builder)
        {
            // The domain events are not persisted here.
            builder.Ignore(p => p.DomainEvents);

            // The primary key is Id.
            builder.HasKey(p => p.Id);

            // The user display name required.
            builder.Property(p => p.UserDisplayName)
                .IsRequired(true);

            // The user display name has a maximum length of 64.
            builder.Property(p => p.UserDisplayName)
                .HasMaxLength(64);

            // The user name required.
            builder.Property(p => p.UserName)
                .IsRequired(true);

            // The user name has a maximum length of 32.
            builder.Property(p => p.UserName)
                .HasMaxLength(32);

            // The user has many kweets.
            builder.HasMany(p => p.Kweets)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Navigation(p => p.Kweets)
                .HasField("kweets")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            // The user has many kweet likes.
            builder.HasMany(p => p.KweetLikes)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(p => p.KweetLikes)
                .HasField("kweetLikes")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}