using System;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public class AuditableDoubleKeyEntity<TFirstId, TSecondId> : DoubleKeyEntityBase<TFirstId, TSecondId>,
        IAuditAbleEntity
    {
        public DateTime UpdatedDate { get; private set; }
        public DateTime CreatedAt { get; }

        protected AuditableDoubleKeyEntity() 
        {
        }
        
        protected AuditableDoubleKeyEntity(TFirstId firstKey, TSecondId secondKey) 
            : base(firstKey, secondKey)
        {
            CreatedAt = DateTime.UtcNow;
        }

        protected void SetUpdatedDate()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}