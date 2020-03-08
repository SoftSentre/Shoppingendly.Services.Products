using System.Collections.Generic;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base
{
    public interface IDomainEventAccessor
    {
        Maybe<IEnumerable<IDomainEvent>> GetUncommitted();
        Task DispatchEventsAsync();
        void ClearAllDomainEvents();
    }
}