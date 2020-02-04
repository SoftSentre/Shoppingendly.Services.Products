using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public abstract class EventSourcingEntity<TId> : EntityBase<TId>, IEventSourcingEntity
    {
        private List<IDomainEvent> _domainEvents;
        
        public IReadOnlyCollection<IDomainEvent> DomainEvents 
            => _domainEvents.AsReadOnly();

        protected EventSourcingEntity(TId id) : base(id) { }
        
        public List<IDomainEvent> GetUncommitted()
        {
            return _domainEvents ??= new List<IDomainEvent>();
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
