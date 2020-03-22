using System.Collections.Generic;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services.Base
{
    public interface IProductDomainService
    {
        Task<Maybe<Product>> GetProductAsync(ProductId productId);
        Task<Maybe<Product>> GetProductWithCategoriesAsync(ProductId productId);
        Task<Maybe<IEnumerable<Product>>> GetProductsByNameAsync(string name);
        Task<Maybe<IEnumerable<Product>>> GetProductsByNameWithCategoriesAsync(string name);
        Task<Maybe<IEnumerable<ProductCategory>>> GetAssignedCategoriesAsync(ProductId productId);
        
        Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string name,
            string producer);
        
        Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string name,
            string producer, IEnumerable<CategoryId> categoryIds);

        Task<bool> AddOrChangeProductPictureAsync(ProductId productId, Picture picture);
        Task RemovePictureFromProductAsync(ProductId productId);
        Task<bool> ChangeProductNameAsync(ProductId productId, string name);
        Task<bool> ChangeProductProducerAsync(ProductId productId, string producer);
        Task AssignProductToCategoryAsync(ProductId productId, CategoryId categoryId);
        Task DeallocateProductFromCategoryAsync(ProductId productId, CategoryId categoryId);
        Task DeallocateProductFromAllCategoriesAsync(ProductId productId);
    }
}