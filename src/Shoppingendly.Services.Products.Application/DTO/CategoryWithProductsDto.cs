using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;

namespace Shoppingendly.Services.Products.Application.DTO
{
    public class CategoryWithProductsDto : CategoryDto
    {
        public IEnumerable<Product> Products { get; }

        public CategoryWithProductsDto(string id, string name, string description, IEnumerable<Product> products) :
            base(id, name, description)
        {
            Products = products;
        }
    }
}