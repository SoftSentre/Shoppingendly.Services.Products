using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services
{
    public class CategoryDomainService : ICategoryDomainService
    {
        public Maybe<Category> CreateNewCategory(CategoryId categoryId, string categoryName)
        {
            var newCategory = Category.Create(categoryId, categoryName);

            return newCategory;
        }

        public Maybe<Category> CreateNewCategory(CategoryId categoryId, string categoryName, string description)
        {
            var newCategory = Category.Create(categoryId, categoryName, description);

            return newCategory;
        }

        public bool SetCategoryName(Maybe<Category> category, string categoryName)
        {
            var validatedCategory = IfCategoryIsEmptyThenThrow(category);
            var isNameChanged = validatedCategory.SetName(categoryName);
            
            return isNameChanged;
        }

        public bool SetCategoryDescription(Maybe<Category> category, string categoryDescription)
        {
            var validatedCategory = IfCategoryIsEmptyThenThrow(category);
            var isDescriptionChanged = validatedCategory.SetDescription(categoryDescription);
            
            return isDescriptionChanged;
        }
        
        private static Category IfCategoryIsEmptyThenThrow(Maybe<Category> category)
        {
            if (category.HasNoValue)
            {
                throw new EmptyCategoryProvidedException(
                    "Unable to mutate category state, because provided value is empty.");
            }

            return category.Value;
        }
    }
}