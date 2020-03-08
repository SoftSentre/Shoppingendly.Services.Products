using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IDomainEvent;
    }
}