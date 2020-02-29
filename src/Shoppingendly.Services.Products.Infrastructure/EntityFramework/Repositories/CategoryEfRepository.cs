using System.Collections.Generic;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories
{
    public class CategoryEfRepository : ICategoryRepository
    {
        private readonly ProductServiceDbContext _productServiceDbContext;

        public CategoryEfRepository(ProductServiceDbContext productServiceDbContext)
        {
            _productServiceDbContext = productServiceDbContext
                .IfEmptyThenThrowAndReturnValue();
        }

        public Task<Maybe<Category>> GetByIdAsync(CategoryId categoryId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Maybe<Category>> GetByNameAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<Maybe<Category>> GetByNameWithIncludesAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<Maybe<IEnumerable<Category>>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Maybe<IEnumerable<Category>>> GetAllWithIncludesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddAsync(Category category)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Category category)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Category category)
        {
            throw new System.NotImplementedException();
        }
    }
}