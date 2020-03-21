using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Categories
{
    public class CategoryDescriptionChangedDomainEvent : DomainEventBase
    {
        public CategoryId CategoryId { get; }
        public string CategoryDescription { get; }

        internal CategoryDescriptionChangedDomainEvent()
        {
            // only for blocking creation of new object in other assembly than this.
        }
        
        internal CategoryDescriptionChangedDomainEvent(CategoryId categoryId, string categoryDescription)
        {
            CategoryId = categoryId;
            CategoryDescription = categoryDescription;
        }
    }
}