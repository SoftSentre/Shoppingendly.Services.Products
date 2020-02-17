using System;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public abstract class AuditableEntity<TId> : EntityBase<TId>, IAuditAbleEntity
    {
        public DateTime UpdatedDate { get; private set; }
        public DateTime CreatedAt { get; }

        protected AuditableEntity()
        {
        }
        
        protected AuditableEntity(TId id) : base(id)
        {
            CreatedAt = DateTime.UtcNow;
        }

        protected void SetUpdatedDate()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}