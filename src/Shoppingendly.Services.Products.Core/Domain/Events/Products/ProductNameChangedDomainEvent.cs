using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Products
{
    public class ProductNameChangedDomainEvent : DomainEventBase
    {
        public ProductId ProductId { get; }
        public string ProductName { get; }

        public ProductNameChangedDomainEvent(ProductId productId, string productName)
        {
            ProductId = productId;
            ProductName = productName;
        }
    }
}