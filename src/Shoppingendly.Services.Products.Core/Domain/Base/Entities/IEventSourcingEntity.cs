using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public interface IEventSourcingEntity
    {
        IEnumerable<IDomainEvent> DomainEvents { get; }
        IEnumerable<IDomainEvent> GetUncommitted();
        void AddDomainEvent(IDomainEvent domainEvent);
        void ClearDomainEvents();
    }
}
