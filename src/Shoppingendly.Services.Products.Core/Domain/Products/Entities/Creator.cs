using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Entities
{
    public class Creator 
    {
        public CreatorId CreatorId { get; }
        public string Name { get; set; }
    }
}