using Kwetter.Services.FollowService.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.FollowService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="UserAggregateConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="FollowService"/> in the DbContext.
    /// </summary>
    internal sealed class UserAggregateConfiguration : IEntityTypeConfiguration<UserAggregate>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<UserAggregate> builder)
        {
            // The domain events are not persisted here.
            builder.Ignore(p => p.DomainEvents);

            // The primary key is the user id.            
            builder.HasKey(p => p.Id);

            // The user display name is required.
            builder.Property(p => p.UserDisplayName)
                .HasMaxLength(64)
                .IsRequired(true);

            // The user profile picture url is required.
            builder.Property(p => p.UserProfilePictureUrl)
                .HasMaxLength(512)
                .IsRequired(true);

            // The user has many followings.
            builder.HasMany(p => p.Followings)
                .WithOne(p => p.Follower)
                .HasForeignKey(p => p.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            // The user has many followers.
            builder.HasMany(p => p.Followers)
                .WithOne(p => p.Following)
                .HasForeignKey(p => p.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Metadata
                .FindNavigation("Followers")
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation("Followings")
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}