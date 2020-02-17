using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories.Base;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories
{
    public class CategoryEfRepository : Repository<Category, CategoryId>, ICategoryRepository
    {
        public CategoryEfRepository(ProductServiceDbContext productServiceDbContext) : base(productServiceDbContext)
        {
        }
    }
}