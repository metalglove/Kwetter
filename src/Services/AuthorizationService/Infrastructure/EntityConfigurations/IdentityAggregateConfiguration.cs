using Kwetter.Services.AuthorizationService.Domain.AggregatesModel.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.AuthorizationService.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Represents the <see cref="IdentityAggregateConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="AuthorizationService"/> in the DbContext.
    /// </summary>
    internal sealed class IdentityAggregateConfiguration : IEntityTypeConfiguration<IdentityAggregate>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<IdentityAggregate> builder)
        {
            // The domain events are not persisted here.
            builder.Ignore(p => p.DomainEvents);

            // The primary key is Id.
            builder.HasKey(p => p.Id);

            // The open id is required.
            builder.Property(p => p.OpenId)
                .IsRequired(true);

            // The email is required.
            builder.Property(p => p.Email)
                .IsRequired(true);

            // The given name is required.
            builder.Property(p => p.GivenName)
                .IsRequired(true);

            // The given name is required.
            builder.Property(p => p.UserName)
                .HasMaxLength(32)
                .IsRequired(true);

            // Ensures that the user name is unqiue.
            builder.HasIndex(p => p.UserName)
                .IsUnique(true);

            // The profi.e picture url is required.
            builder.Property(p => p.ProfilePictureUrl)
                .IsRequired(true);
        }
    }
}
