using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services
{
    public class ProductDomainService : IProductDomainService
    {
        private readonly IProductRepository _productRepository;

        public ProductDomainService(IProductRepository productRepository)
        {
            _productRepository = productRepository
                .IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Product>> GetProductAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }

        public async Task<Maybe<Product>> GetProductByNameAsync(string name)
        {
            var product = await _productRepository.GetByNameAsync(name);
            return product;
        }

        public async Task<Maybe<Product>> GetProductByNameWithCategoriesAsync(string name)
        {
            var product = await _productRepository.GetByNameWithIncludesAsync(name);
            return product;
        }

        public async Task<Maybe<IEnumerable<ProductCategory>>> GetAssignedCategoriesAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            var assignedCategories = validatedProduct.GetAllAssignedCategories();

            return assignedCategories;
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string name,
            string producer)
        {
            var newProduct = Product.Create(productId, creatorId, name, producer);
            await _productRepository.AddAsync(newProduct);

            return newProduct;
        }
        
        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string name,
            string producer, IEnumerable<CategoryId> categoryIds)
        {
            var newProduct = Product.Create(productId, creatorId, name, producer);
            var categoryIdsAsList = categoryIds.ToList();

            if (categoryIdsAsList.Any())
                categoryIdsAsList.ForEach(ci => AssignProduct(newProduct, ci));

            await _productRepository.AddAsync(newProduct);
            return newProduct;
        }

        public async Task<bool> AddOrChangeProductPictureAsync(ProductId productId, Picture picture)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var validatedProduct = IfProductIsEmptyThenThrow(product.Value);
            var isPictureChanged = validatedProduct.AddOrChangePicture(picture);

            return isPictureChanged;
        }

        public async Task RemovePictureFromProductAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            validatedProduct.RemovePicture();
        }

        public async Task<bool> ChangeProductNameAsync(ProductId productId, string name)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            var isNameChanged = validatedProduct.SetName(name);

            return isNameChanged;
        }

        public async Task<bool> ChangeProductProducerAsync(ProductId productId, string producer)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            var isProducerChanged = validatedProduct.SetProducer(producer);

            return isProducerChanged;
        }

        public async Task AssignProductToCategoryAsync(ProductId productId, CategoryId categoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            AssignProduct(product, categoryId);
        }

        public async Task DeallocateProductFromCategoryAsync(ProductId productId, CategoryId categoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var validatedProduct = IfProductIsEmptyThenThrow(product);
            
            validatedProduct.DeallocateCategory(categoryId);
        }

        public async Task DeallocateProductFromAllCategoriesAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
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