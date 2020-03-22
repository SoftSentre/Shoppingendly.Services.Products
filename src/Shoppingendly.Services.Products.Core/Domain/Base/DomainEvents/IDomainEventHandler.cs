using System.Threading.Tasks;

namespace Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents
{
    public interface IDomainEventHandler<in TEvent> 
        where TEvent : class, IDomainEvent
    {
        Task HandleAsync(TEvent @event);
    }
}