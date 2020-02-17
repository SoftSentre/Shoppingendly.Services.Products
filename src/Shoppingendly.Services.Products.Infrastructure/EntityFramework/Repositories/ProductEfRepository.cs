using System.Threading.Tasks;
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

        public Task<Maybe<Product>> GetByIdAsync(ProductId productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Maybe<Product>> GetByNameAsync(ProductId productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Maybe<Product>> GetByNameWithIncludesAsync(ProductId productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddAsync(Product product)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}