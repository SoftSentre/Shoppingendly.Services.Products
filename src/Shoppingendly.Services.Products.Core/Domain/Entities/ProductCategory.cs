using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Entities
{
    public class ProductCategory : AuditableDoubleKeyEntity<ProductId, CategoryId>
    {
        // Navigation property
        public Product Product { get; set; }
        
        // Navigation property
        public Category Category { get; set; }

        private ProductCategory()
        {
            // Required for EF
        }

        internal ProductCategory(ProductId productId, CategoryId categoryId) 
            : base(productId, categoryId)
        {
        }

        internal static ProductCategory Create(ProductId productId, CategoryId categoryId)
        {
            return new ProductCategory(productId, categoryId);
        }
    }
}