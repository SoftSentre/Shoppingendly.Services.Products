using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Products
{
    public class NewProductCreatedDomainEvent : DomainEventBase
    {
        public ProductId ProductId { get; }
        public CreatorId CreatorId { get; }
        public string ProductName { get; }
        public string ProductProducer { get; }

        public NewProductCreatedDomainEvent(ProductId productId, CreatorId creatorId, string productName,
            string productProducer)
        {
            ProductId = productId;
            CreatorId = creatorId;
            ProductName = productName;
            ProductProducer = productProducer;
        }
    }
}