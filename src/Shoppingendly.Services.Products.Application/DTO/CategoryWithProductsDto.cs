using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;

namespace Shoppingendly.Services.Products.Application.DTO
{
    public class CategoryWithProductsDto : CategoryDto
    {
        public IEnumerable<ProductDto> Products { get; }

        public CategoryWithProductsDto(string id, string name, string description, IEnumerable<ProductDto> products) :
            base(id, name, description)
        {
            Products = products;
        }
    }
}