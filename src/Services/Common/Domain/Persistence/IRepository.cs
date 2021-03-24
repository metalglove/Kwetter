namespace Kwetter.Services.Common.Domain.Persistence
{
    /// <summary>
    /// Represents the <see cref="IRepository{TAggregate}"/> interface.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate root.</typeparam>
    public interface IRepository<TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// Gets the unit of work for the repository.
        /// </summary>
        public IUnitOfWork UnitOfWork { get; }
    }
}
