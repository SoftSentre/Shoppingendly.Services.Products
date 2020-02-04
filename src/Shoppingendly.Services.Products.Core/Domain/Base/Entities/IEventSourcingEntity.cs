using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public interface IEventSourcingEntity
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        List<IDomainEvent> GetUncommitted();
        void AddDomainEvent(IDomainEvent domainEvent);
        void ClearDomainEvents();
    }
}
