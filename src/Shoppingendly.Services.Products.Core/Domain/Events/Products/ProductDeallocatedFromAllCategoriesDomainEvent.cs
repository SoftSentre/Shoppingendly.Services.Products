using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Products
{
    public class ProductDeallocatedFromAllCategoriesDomainEvent : DomainEventBase
    {
        public ProductId ProductId { get; }
        public IEnumerable<CategoryId> CategoriesIds { get; }

        internal ProductDeallocatedFromAllCategoriesDomainEvent(ProductId productId, IEnumerable<CategoryId> categoriesIds)
        {
            ProductId = productId;
            CategoriesIds = categoriesIds;
        }
    }
}