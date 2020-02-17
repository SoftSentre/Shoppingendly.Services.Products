using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Categories
{
    public class NewCategoryCreatedDomainEvent : DomainEventBase
    {
        public CategoryId CategoryId { get; }
        public string CategoryName { get; }
        public string CategoryDescription { get; }

        public NewCategoryCreatedDomainEvent(CategoryId categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }
        
        public NewCategoryCreatedDomainEvent(CategoryId categoryId, string categoryName, string categoryDescription)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
        }
    }
}