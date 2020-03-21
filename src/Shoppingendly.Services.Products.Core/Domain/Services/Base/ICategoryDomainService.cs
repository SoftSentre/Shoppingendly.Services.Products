using System.Collections.Generic;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services.Base
{
    public interface ICategoryDomainService
    {
        Task<Maybe<Category>> GetCategoryAsync(CategoryId categoryId);
        Task<Maybe<Category>> GetCategoryByNameAsync(string categoryName);
        Task<Maybe<Category>> GetCategoryWithProductsAsync(string categoryName);
        Task<Maybe<IEnumerable<Category>>> GetAllCategoriesAsync();
        Task<Maybe<IEnumerable<Category>>> GetAllCategoriesWithProductsAsync();
        Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId, string categoryName);
        Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId, 
            string categoryName, string description);
        
        Task<bool> SetCategoryNameAsync(CategoryId categoryId, string categoryName);
        Task<bool> SetCategoryDescriptionAsync(CategoryId categoryId, string categoryDescription);
    }
}