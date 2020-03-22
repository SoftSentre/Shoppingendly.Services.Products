using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Products
{
    public class PictureAddedOrChangedDomainEvent : DomainEventBase
    {
        public ProductId ProductId { get; }
        public Picture Picture { get; }

        internal PictureAddedOrChangedDomainEvent(ProductId productId, Picture picture)
        {
            ProductId = productId;
            Picture = picture;
        }
    }
}