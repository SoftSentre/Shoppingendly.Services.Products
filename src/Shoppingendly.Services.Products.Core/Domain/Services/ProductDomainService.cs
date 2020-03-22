using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services.Products;
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

        public async Task<Maybe<Product>> GetProductWithCategoriesAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdWithIncludesAsync(productId);
            return product;
        }

        public async Task<Maybe<IEnumerable<Product>>> GetProductsByNameAsync(string name)
        {
            var products = await _productRepository.GetManyByNameAsync(name);
            return products;
        }

        public async Task<Maybe<IEnumerable<Product>>> GetProductsByNameWithCategoriesAsync(string name)
        {
            var product = await _productRepository.GetManyByNameWithIncludesAsync(name);
            return product;
        }

        public async Task<Maybe<IEnumerable<ProductCategory>>> GetAssignedCategoriesAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdWithIncludesAsync(productId).UnwrapAsync(
                new ProductNotFoundException(
                    $"Unable to mutate product state, because product with id: {productId} is empty."));

            var assignedCategories = product.GetAllAssignedCategories();

            return assignedCategories;
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string name,
            string producer)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product.HasValue)
            {
                throw new ProductAlreadyExistsException(
                    $"Unable to add new product, because product with id: {productId} is already exists.");
            }

            var newProduct = Product.Create(productId, creatorId, name, producer);
            await _productRepository.AddAsync(newProduct);

            return newProduct;
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string name,
            string producer, IEnumerable<CategoryId> categoryIds)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product.HasValue)
            {
                throw new ProductAlreadyExistsException(
                    $"Unable to add new product, because product with id: {productId} is already exists.");
            }

            var newProduct = Product.Create(productId, creatorId, name, producer);
            var categoryIdsAsList = categoryIds.ToList();

            if (categoryIdsAsList.Any())
                categoryIdsAsList.ForEach(ci => newProduct.AssignCategory(ci));

            await _productRepository.AddAsync(newProduct);
            return newProduct;
        }

        public async Task<bool> AddOrChangeProductPictureAsync(ProductId productId, Picture picture)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(
                    $"Unable to mutate product state, because product with id: {productId} is empty."));
            
            var isPictureChanged = product.AddOrChangePicture(picture);

            if (isPictureChanged)
                _productRepository.Update(product);

            return isPictureChanged;
        }

        public async Task RemovePictureFromProductAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(
                    $"Unable to mutate product state, because product with id: {productId} is empty."));
            
            product.RemovePicture();
            _productRepository.Update(product);
        }

        public async Task<bool> ChangeProductNameAsync(ProductId productId, string name)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(
                    $"Unable to mutate product state, because product with id: {productId} is empty."));
            
            var isNameChanged = product.SetName(name);

            if (isNameChanged)
                _productRepository.Update(product);

            return isNameChanged;
        }

        public async Task<bool> ChangeProductProducerAsync(ProductId productId, string producer)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(
                    $"Unable to mutate product state, because product with id: {productId} is empty."));

            var isProducerChanged = product.SetProducer(producer);

            if (isProducerChanged)
                _productRepository.Update(product);

            return isProducerChanged;
        }

        public async Task AssignProductToCategoryAsync(ProductId productId, CategoryId categoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(
                $"Unable to mutate product state, because product with id: {productId} is empty."));

            product.AssignCategory(categoryId);
            _productRepository.Update(product);
        }

        public async Task DeallocateProductFromCategoryAsync(ProductId productId, CategoryId categoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(
                    $"Unable to mutate product state, because product with id: {productId} is empty."));
            
            product.DeallocateCategory(categoryId);
            _productRepository.Update(product);
        }

        public async Task DeallocateProductFromAllCategoriesAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(
                    $"Unable to mutate product state, because product with id: {productId} is empty."));
            
            product.DeallocateAllCategories();
            _productRepository.Update(product);
        }
    }
}