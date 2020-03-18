using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base
{
    public interface IDomainEventAccessor
    {
        Maybe<IEnumerable<IDomainEvent>> GetUncommittedEvents();
        void DispatchEvents(IEnumerable<IDomainEvent> domainEvents);
        void ClearAllDomainEvents();
    }
}