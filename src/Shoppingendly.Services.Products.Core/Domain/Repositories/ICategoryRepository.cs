using Shoppingendly.Services.Products.Core.Domain.Base.SeedWork;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category, CategoryId>
    {
    }
}