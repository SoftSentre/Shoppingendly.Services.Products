using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;

namespace Shoppingendly.Services.Products.Application.DTO
{
    public class CreatorWithProducts : CreatorDto
    {
        public IEnumerable<Product> Products { get; set; }
    }
}