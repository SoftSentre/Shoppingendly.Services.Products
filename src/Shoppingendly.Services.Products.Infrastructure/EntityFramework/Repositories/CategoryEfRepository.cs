using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Maybe<Category>> GetByIdAsync(CategoryId categoryId)
        {
            return await _productServiceDbContext.Categories.FirstOrDefaultAsync(c => c.Id.Equals(categoryId));
        }

        public async Task<Maybe<Category>> GetByNameAsync(string name)
        {
            return await _productServiceDbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Maybe<Category>> GetByNameWithIncludesAsync(string name)
        {
            return await _productServiceDbContext.Categories.Include(c => c.ProductCategories)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Maybe<IEnumerable<Category>>> GetAllAsync()
        {
            return await _productServiceDbContext.Categories.ToListAsync();
        }

        public async Task<Maybe<IEnumerable<Category>>> GetAllWithIncludesAsync()
        {
            return await _productServiceDbContext.Categories.Include(c => c.ProductCategories).ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _productServiceDbContext.AddAsync(category);
        }

        public void Update(Category category)
        {
            _productServiceDbContext.Update(category);
        }

        public void Delete(Category category)
        {
            _productServiceDbContext.Remove(category);
        }
    }
}