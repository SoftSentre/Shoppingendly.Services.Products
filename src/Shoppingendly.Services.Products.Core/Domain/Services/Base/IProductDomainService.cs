using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services.Base
{
    public interface IProductDomainService
    {
        Maybe<ProductCategory> GetAssignedCategory(Maybe<Product> product, CategoryId categoryId);
        Maybe<IEnumerable<ProductCategory>> GetAssignedCategories(Maybe<Product> product);
        
        Maybe<Product> AddNewProduct(ProductId productId, CreatorId creatorId, string name,
            string producer);
        
        Maybe<Product> AddNewProduct(ProductId productId, CreatorId creatorId, string name,
            string producer, IEnumerable<CategoryId> categoryIds);

        bool AddOrChangeProductPicture(Maybe<Product> product, Picture picture);
        void RemovePictureFromProduct(Maybe<Product> product);
        bool ChangeProductName(Maybe<Product> product, string name);
        bool ChangeProductProducer(Maybe<Product> product, string producer);
        void AssignProductToCategory(Maybe<Product> product, CategoryId categoryId);
        void DeallocateProductFromCategory(Maybe<Product> product, CategoryId categoryId);
        void DeallocateProductFromAllCategories(Maybe<Product> product);
    }
}