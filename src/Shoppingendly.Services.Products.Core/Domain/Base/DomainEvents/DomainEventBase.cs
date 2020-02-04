using System;

namespace Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccuredAt { get; }

        protected DomainEventBase()
        {
            Id = Guid.NewGuid();
            OccuredAt = DateTime.UtcNow;
        }
    }
}
