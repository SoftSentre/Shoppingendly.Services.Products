using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents
{
    public class DomainEventsPublisher : IDomainEventPublisher
    {
        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            throw new System.NotImplementedException();
        }
    }
}