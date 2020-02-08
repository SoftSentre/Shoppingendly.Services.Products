using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Categories
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