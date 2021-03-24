using Kwetter.Services.Common.Domain.Events;
using System;
using System.Collections.Generic;

namespace Kwetter.Services.Common.Domain
{
    /// <summary>
    /// Represents the <see cref="Entity"/> class.
    /// This base class holds the functionality to perform eventing on domain aggregates.
    /// </summary>
    public abstract class Entity
    {
        private List<DomainEvent> _domainEvents;
        private int? _requestedHashCode;

        /// <summary>
        /// Gets and sets the id of the entity.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Gets the domain events.
        /// </summary>
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// Adds a domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents ??= new List<DomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Removes a domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        public void RemoveDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        /// <summary>
        /// Clears the domain events.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        /// <summary>
        /// Checks whether the entity is transient (has a known identifier).
        /// </summary>
        /// <returns>Returns a boolean indicating whether the entity is transient.</returns>
        public bool IsTransient()
        {
            return Id == default;
        }

        /// <inheritdoc cref="Object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (GetType() != obj.GetType())
                return false;
            Entity item = (Entity)obj;
            if (item.IsTransient() || IsTransient())
                return false;
            return item.Id == Id;
        }

        /// <inheritdoc cref="Object.GetHashCode()"/>
        public override int GetHashCode()
        {
            if (IsTransient()) 
                return base.GetHashCode();
            _requestedHashCode ??= Id.GetHashCode() ^ 31;
            return _requestedHashCode.Value;
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
