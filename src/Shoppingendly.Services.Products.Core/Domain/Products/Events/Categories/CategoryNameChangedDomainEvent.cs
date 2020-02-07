using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Events.Categories
{
    public class CategoryNameChangedDomainEvent : DomainEventBase
    {
        public CategoryId CategoryId { get; }
        public string CategoryName { get; }

        public CategoryNameChangedDomainEvent(CategoryId categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }
    }
}