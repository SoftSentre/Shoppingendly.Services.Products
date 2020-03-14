using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;

namespace Shoppingendly.Services.Products.Application.DTO
{
    public class CreatorWithProductsDto : CreatorDto
    {
        public IEnumerable<ProductDto> Products { get; }

        public CreatorWithProductsDto(string id, string name, RoleDto role, IEnumerable<ProductDto> products)
            : base(id, name, role)
        {
            Products = products;
        }
    }
}