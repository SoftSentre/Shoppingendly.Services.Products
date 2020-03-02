using System.Collections.Generic;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Maybe<Category>> GetByIdAsync(CategoryId categoryId);
        Task<Maybe<Category>> GetByNameAsync(string name);
        Task<Maybe<Category>> GetByNameWithIncludesAsync(string name);
        Task<Maybe<IEnumerable<Category>>> GetAllAsync();
        Task<Maybe<IEnumerable<Category>>> GetAllWithIncludesAsync();
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}