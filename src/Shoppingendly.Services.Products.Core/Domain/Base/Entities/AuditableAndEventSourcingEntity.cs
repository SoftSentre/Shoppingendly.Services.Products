using System;
using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public abstract class AuditableAndEventSourcingEntity<TId> : EntityBase<TId>, IEventSourcingEntity, IAuditAbleEntity
    {
        private List<IDomainEvent> _domainEvents;

        public DateTime UpdatedDate { get; private set; }
        public DateTime CreatedAt { get; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents
            => _domainEvents.AsReadOnly();

        protected AuditableAndEventSourcingEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }
        
        protected AuditableAndEventSourcingEntity(TId id) : base(id)
        {
            CreatedAt = DateTime.UtcNow;
        }

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

        protected void SetUpdatedDate()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}