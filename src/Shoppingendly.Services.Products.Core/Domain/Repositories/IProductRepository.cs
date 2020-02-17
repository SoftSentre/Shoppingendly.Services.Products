using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Repositories
{
    public interface IProductRepository 
    {
        Task<Maybe<Product>> GetByIdAsync(ProductId productId);
        Task<Maybe<Product>> GetByNameAsync(ProductId productId);
        Task<Maybe<Product>> GetByNameWithIncludesAsync(ProductId productId);
        Task<bool> AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}