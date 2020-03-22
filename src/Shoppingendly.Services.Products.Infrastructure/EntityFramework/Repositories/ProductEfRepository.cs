using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories
{
    public class ProductEfRepository : IProductRepository
    {
        private readonly ProductServiceDbContext _productServiceDbContext;

        public ProductEfRepository(ProductServiceDbContext productServiceDbContext)
        {
            _productServiceDbContext = productServiceDbContext
                .IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Product>> GetByIdAsync(ProductId productId)
        {
            return await _productServiceDbContext.Products.FirstOrDefaultAsync(p => p.Id.Equals(productId));
        }

        public async Task<Maybe<Product>> GetByIdWithIncludesAsync(ProductId productId)
        {
            return await _productServiceDbContext.Products.Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id.Equals(productId));
        }

        public async Task<Maybe<IEnumerable<Product>>> GetManyByNameAsync(string name)
        {
            return await _productServiceDbContext.Products
                .Where(p => p.Name == name).ToListAsync();
        }

        public async Task<Maybe<IEnumerable<Product>>> GetManyByNameWithIncludesAsync(string name)
        {
            return await _productServiceDbContext.Products.Include(p => p.ProductCategories)
                .Where(p => p.Name == name).ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _productServiceDbContext.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _productServiceDbContext.Update(product);
        }

        public void Delete(Product product)
        {
            _productServiceDbContext.Remove(product);
        }
    }
}