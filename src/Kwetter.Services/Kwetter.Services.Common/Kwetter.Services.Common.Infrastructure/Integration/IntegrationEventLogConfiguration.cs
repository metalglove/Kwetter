using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Services.Common.Infrastructure.Integration
{
    /// <summary>
    /// Represents the <see cref="IntegrationEventLogConfiguration"/> class used to configure the relations and columns
    /// in the <see cref="DbSet{TEntity}"/> for <see cref="IntegrationEventLogEntry"/> in the DbContext.
    /// </summary>
    internal sealed class IntegrationEventLogConfiguration : IEntityTypeConfiguration<IntegrationEventLogEntry>
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TEntity}.Configure"/>
        public void Configure(EntityTypeBuilder<IntegrationEventLogEntry> builder)
        {
            // The integration event is persisted as string instead of object.
            builder.Ignore(p => p.IntegrationEvent);

            // The short name of the integration is also ignored.
            builder.Ignore(p => p.EventTypeShortName);

            // The primary key is Id.
            builder.HasKey(p => p.EventId);
            
            // The serialized integration event is required.
            builder.Property(p => p.Content)
                .IsRequired(true);

            // The creation time is required.
            builder.Property(p => p.CreationDateTime)
                .IsRequired(true);

            // The event type name is required.
            builder.Property(p => p.EventTypeName)
                .IsRequired(true);

            // The integration event state is required.
            builder.Property(p => p.State)
                .IsRequired(true);

            // The times sent is required.
            builder.Property(p => p.TimesSent)
                .IsRequired(true);

            // The transaction id is required.
            builder.Property(p => p.TransactionId)
                .IsRequired(true);
        }
    }
}
