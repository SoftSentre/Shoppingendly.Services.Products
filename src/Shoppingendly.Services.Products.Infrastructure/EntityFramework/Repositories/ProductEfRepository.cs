using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories.Base;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories
{
    public class ProductEfRepository : Repository<Product, ProductId>, IProductRepository
    {
        public ProductEfRepository(ProductServiceDbContext productServiceDbContext) : base(productServiceDbContext)
        {
        }
    }
}