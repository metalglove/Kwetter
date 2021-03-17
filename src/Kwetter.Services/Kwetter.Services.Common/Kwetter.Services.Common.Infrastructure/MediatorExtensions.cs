using Kwetter.Services.Common.Domain;
using Kwetter.Services.Common.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.Services.Common.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="MediatorExtensions"/> class for dispatching domain events.
    /// </summary>
    public static class MediatorExtensions
    {
        /// <summary>
        /// Dispatches the domain events asynchronously.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Returns an awaitable task.</returns>
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext dbContext)
        {
            IEnumerable<EntityEntry<Entity>> domainEntities = dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            List<DomainEvent> domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (DomainEvent domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
