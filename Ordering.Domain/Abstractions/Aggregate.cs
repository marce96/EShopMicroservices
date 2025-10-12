
namespace Ordering.Domain.Abstractions
{
    public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        public IDomainEvent[] ClearDomainEvent()
        {
            IDomainEvent[] dequeuedEvent = [.. _domainEvents];
            _domainEvents.Clear();
            return dequeuedEvent;
        }
    }
}
