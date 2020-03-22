using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Products
{
    public class ProductDeallocatedFromCategoryDomainEvent : DomainEventBase
    {
        public ProductId ProductId { get; }
        public CategoryId CategoryId { get; }

        internal ProductDeallocatedFromCategoryDomainEvent(ProductId productId, CategoryId categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
    }
}