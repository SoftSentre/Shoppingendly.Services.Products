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
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            var assignedCategory = validatedProduct.GetAssignedCategory(categoryId);

            return assignedCategory;
        }

        public Maybe<IEnumerable<ProductCategory>> GetAssignedCategories(Maybe<Product> product)
        {
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            var assignedCategories = validatedProduct.GetAllAssignedCategories();

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

        public bool AddOrChangeProductPicture(Maybe<Product> product, Picture picture)
        {
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            var isPictureChanged = validatedProduct.AddOrChangePicture(picture);

            return isPictureChanged;
        }

        public void RemovePictureFromProduct(Maybe<Product> product)
        {
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            validatedProduct.RemovePicture();
        }

        public bool ChangeProductName(Maybe<Product> product, string name)
        {
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            var isNameChanged = validatedProduct.SetName(name);

            return isNameChanged;
        }

        public bool ChangeProductProducer(Maybe<Product> product, string producer)
        {
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            var isProducerChanged = validatedProduct.SetProducer(producer);

            return isProducerChanged;
        }

        public void AssignProductToCategory(Maybe<Product> product, CategoryId categoryId)
        {
            AssignProduct(product, categoryId);
        }

        public void DeallocateProductFromCategory(Maybe<Product> product, CategoryId categoryId)
        {
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            
            validatedProduct.DeallocateCategory(categoryId);
        }

        public void DeallocateProductFromAllCategories(Maybe<Product> product)
        {
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            
            validatedProduct.DeallocateAllCategories();
        }

        private static void AssignProduct(Maybe<Product> product, CategoryId categoryId)
        {
            var validatedProduct = IfProductIsEmptyThenThrow(product);

            validatedProduct.AssignCategory(categoryId);
        }

        private static Product IfProductIsEmptyThenThrow(Maybe<Product> product)
        {
            if (product.HasNoValue)
            {
                throw new EmptyProductProvidedException(
                    "Unable to mutate product state, because provided value is empty.");
            }

            return product.Value;
        }
    }
}