using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Products
{
    public class ProductProducerChangedDomainEvent : DomainEventBase
    {
        public ProductId ProductId { get; }
        public string ProductProducer { get; }

        public ProductProducerChangedDomainEvent(ProductId productId, string productProducer)
        {
            ProductId = productId;
            ProductProducer = productProducer;
        }
    }
}