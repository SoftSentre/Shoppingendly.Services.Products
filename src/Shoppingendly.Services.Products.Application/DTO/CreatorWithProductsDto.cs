using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;

namespace Shoppingendly.Services.Products.Application.DTO
{
    public class CreatorWithProductsDto : CreatorDto
    {
        public IEnumerable<Product> Products { get; }

        public CreatorWithProductsDto(string id, string name, RoleDto role, IEnumerable<Product> products)
            : base(id, name, role)
        {
            Products = products;
        }
    }
}