using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base
{
    public interface IDomainEventHandler<in TEvent> 
        where TEvent : class, IDomainEvent
    {
        Task HandleAsync(TEvent @event);
    }
}