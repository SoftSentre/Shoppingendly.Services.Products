using Shoppingendly.Services.Products.Core.Domain.Base.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Aggregates
{
    public class Product : AuditableAndEventSourcingEntity<ProductId>, IAggregateRoot
    {
        public CreatorId CreatorId { get; }
        public string Name { get; private set; }
        public string Producer { get; private set; }

        public Product(ProductId id) : base(id)
        {
        }
    }
}