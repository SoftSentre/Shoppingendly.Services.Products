using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Products
{
    public class PictureRemovedDomainEvent : DomainEventBase
    {
        public ProductId ProductId { get; }

        internal PictureRemovedDomainEvent(ProductId productId)
        {
            ProductId = productId;
        }
    }
}