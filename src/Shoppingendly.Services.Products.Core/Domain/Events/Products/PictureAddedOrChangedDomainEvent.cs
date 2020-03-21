using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Products
{
    public class PictureAddedOrChangedDomainEvent : DomainEventBase
    {
        public ProductId ProductId { get; }
        public Picture Picture { get; }
        
        internal PictureAddedOrChangedDomainEvent()
        {
            // only for blocking creation of new object in other assembly than this.
        }
        
        internal PictureAddedOrChangedDomainEvent(ProductId productId, Picture picture)
        {
            ProductId = productId;
            Picture = picture;
        }
    }
}