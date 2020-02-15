using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services.Base
{
    public interface ICategoryDomainService
    {
        Maybe<Category> CreateNewCategory(CategoryId categoryId, string categoryName);
        Maybe<Category> CreateNewCategory(CategoryId categoryId, 
            string categoryName, string description);
        
        bool SetCategoryName(Maybe<Category> category, string categoryName);
        bool SetCategoryDescription(Maybe<Category> category, string categoryDescription);
    }
}