using System;
using Kwetter.Services.UserService.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.UserService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="UserAggregateConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="UserService"/> in the DbContext.
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
            
            // The display name is unique.
            builder.HasIndex(p => p.DisplayName)
                .IsUnique(true);

            // The username is required.
            builder.Property(p => p.DisplayName)
                .IsRequired(true);

            // The username has a maximum length of 64.
            builder.Property(p => p.DisplayName)
                .HasMaxLength(64);

            // Configures the navigation property for the UserAggregate.
            builder.OwnsOne(p => p.Profile, navigationBuilder =>
            {
                navigationBuilder.Ignore(p => p.DomainEvents);

                // This is an owned entity type so the id can be ignored.
                navigationBuilder.Ignore(p => p.Id);

                // The profile description has a maximum length of 512.
                navigationBuilder.Property(p => p.Description)
                    .HasMaxLength(512);

                // The description is required.
                navigationBuilder.Property(p => p.Description)
                    .IsRequired(true);

                // UserAggregate is the owner.
                navigationBuilder.WithOwner();
            });
        }
    }
}
