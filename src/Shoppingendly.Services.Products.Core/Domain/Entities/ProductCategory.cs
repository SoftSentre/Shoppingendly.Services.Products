using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.ProductCategories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Entities
{
    public class ProductCategory : AuditableAndEventSourcingDoubleKeyEntity<ProductId, CategoryId>
    {
        public Product Product { get; private set; }
        public Category Category { get; private set; }

        private ProductCategory()
        {
            // Required for EF
        }

        public ProductCategory(ProductId productId, CategoryId categoryId) 
            : base(productId, categoryId)
        {
            AddDomainEvent(new ProductAssignedToCategoryDomainEvent(productId, categoryId));
        }
    }
}