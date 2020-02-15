using System.Collections.Generic;
using System.Linq;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services
{
    public class ProductDomainService : IProductDomainService
    {
        public Maybe<ProductCategory> GetAssignedCategory(Maybe<Product> product, CategoryId categoryId)
        {
            IfProductIsEmptyThenThrow(product);
            var assignedCategory = product.Value.GetAssignedCategory(categoryId);

            return assignedCategory;
        }

        public Maybe<IEnumerable<ProductCategory>> GetAssignedCategories(Maybe<Product> product)
        {
            IfProductIsEmptyThenThrow(product);
            var assignedCategories = product.Value.GetAllAssignedCategories();

            return assignedCategories;
        }

        public Maybe<Product> AddNewProduct(ProductId productId, CreatorId creatorId, string name,
            string producer)
        {
            var newProduct = Product.Create(productId, creatorId, name, producer);

            return newProduct;
        }
        
        public Maybe<Product> AddNewProduct(ProductId productId, CreatorId creatorId, string name,
            string producer, IEnumerable<CategoryId> categoryIds)
        {
            var newProduct = Product.Create(productId, creatorId, name, producer);
            var categoryIdsAsList = categoryIds.ToList();

            if (categoryIdsAsList.Any())
                categoryIdsAsList.ForEach(ci => AssignProduct(newProduct, ci));

            return newProduct;
        }

        public bool ChangeProductName(Maybe<Product> product, string name)
        {
            IfProductIsEmptyThenThrow(product);
            var isNameChanged = product.Value.SetName(name);

            return isNameChanged;
        }

        public bool ChangeProductProducer(Maybe<Product> product, string producer)
        {
            IfProductIsEmptyThenThrow(product);
            var isProducerChanged = product.Value.SetProducer(producer);

            return isProducerChanged;
        }

        public void AssignProductToCategory(Maybe<Product> product, CategoryId categoryId)
        {
            AssignProduct(product, categoryId);
        }

        public void DeallocateProductFromCategory(Maybe<Product> product, CategoryId categoryId)
        {
            IfProductIsEmptyThenThrow(product);
            
            product.Value.DeallocateCategory(categoryId);
        }

        public void DeallocateProductFromAllCategories(Maybe<Product> product)
        {
            IfProductIsEmptyThenThrow(product);
            
            product.Value.DeallocateAllCategories();
        }

        private static void AssignProduct(Maybe<Product> product, CategoryId categoryId)
        {
            IfProductIsEmptyThenThrow(product);

            product.Value.AssignCategory(categoryId);
        }

        private static void IfProductIsEmptyThenThrow(Maybe<Product> product)
        {
            if (product.HasNoValue)
            {
                throw new EmptyProductProvidedException(
                    "Unable to mutate product state, because provided value is empty.", product);
            }
        }
    }
}