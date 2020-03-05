using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Repositories
{
    public interface IProductRepository 
    {
        Task<Maybe<Product>> GetByIdAsync(ProductId productId);
        Task<Maybe<Product>> GetByNameAsync(string name);
        Task<Maybe<Product>> GetByNameWithIncludesAsync(string name);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}