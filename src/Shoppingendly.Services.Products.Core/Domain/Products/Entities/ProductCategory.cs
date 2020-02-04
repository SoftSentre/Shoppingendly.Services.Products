using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Entities
{
    public class ProductCategory
    {
        public ProductId ProductId { get; set; }
        public CategoryId CategoryId { get; set; }
    }
}