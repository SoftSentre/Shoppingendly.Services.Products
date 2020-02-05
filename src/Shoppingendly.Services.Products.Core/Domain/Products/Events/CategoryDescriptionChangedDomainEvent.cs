using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Events
{
    public class CategoryDescriptionChangedDomainEvent : DomainEventBase
    {
        public CategoryId CategoryId { get; }
        public string CategoryDescription { get; }
        
        public CategoryDescriptionChangedDomainEvent(CategoryId categoryId, string categoryDescription)
        {
            CategoryId = categoryId;
            CategoryDescription = categoryDescription;
        }
    }
}