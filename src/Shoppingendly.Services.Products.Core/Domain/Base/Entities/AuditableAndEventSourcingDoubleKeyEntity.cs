using System;
using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public class AuditableAndEventSourcingDoubleKeyEntity<TFirstId, TSecondId> 
        : DoubleKeyEntityBase<TFirstId, TSecondId>, IEventSourcingEntity, IAuditAbleEntity
    {
        private List<IDomainEvent> _domainEvents;

        public DateTime? UpdatedDate { get; private set; }
        public DateTime CreatedAt { get; }

        public IEnumerable<IDomainEvent> DomainEvents
            => _domainEvents.AsReadOnly();

        protected AuditableAndEventSourcingDoubleKeyEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        protected AuditableAndEventSourcingDoubleKeyEntity(TFirstId firstKey, TSecondId secondKey) 
            : base(firstKey, secondKey)
        {
            CreatedAt = DateTime.UtcNow;
        }

        public IEnumerable<IDomainEvent> GetUncommitted()
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

        protected void SetUpdatedDate()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}