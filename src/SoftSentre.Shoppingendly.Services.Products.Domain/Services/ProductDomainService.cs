// Copyright 2020 SoftSentre Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Services
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
                new ProductNotFoundException(productId));

            var assignedCategories = product.GetAllAssignedCategories();

            return assignedCategories;
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string name,
            ProductProducer producer)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product.HasValue)
            {
                throw new ProductAlreadyExistsException(productId);
            }

            var newProduct = Product.Create(productId, creatorId, name, producer);
            await _productRepository.AddAsync(newProduct);

            return newProduct;
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string name,
            ProductProducer producer, IEnumerable<CategoryId> categoryIds)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product.HasValue)
            {
                throw new ProductAlreadyExistsException(productId);
            }

            var newProduct = Product.Create(productId, creatorId, name, producer);
            var categoryIdsAsList = categoryIds.ToList();

            if (categoryIdsAsList.Any())
            {
                categoryIdsAsList.ForEach(ci => newProduct.AssignCategory(ci));
            }

            await _productRepository.AddAsync(newProduct);
            return newProduct;
        }

        public async Task<bool> AddOrChangeProductPictureAsync(ProductId productId, Picture productPicture)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(productId));

            var isPictureChanged = product.AddOrChangeProductPicture(productPicture);

            if (isPictureChanged)
            {
                _productRepository.Update(product);
            }

            return isPictureChanged;
        }

        public async Task<bool> ChangeProductNameAsync(ProductId productId, string name)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(productId));

            var isNameChanged = product.SetProductName(name);

            if (isNameChanged)
            {
                _productRepository.Update(product);
            }

            return isNameChanged;
        }

        public async Task<bool> ChangeProductProducerAsync(ProductId productId, ProductProducer productProducer)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(productId));

            var isProducerChanged = product.SetProductProducer(productProducer);

            if (isProducerChanged)
            {
                _productRepository.Update(product);
            }

            return isProducerChanged;
        }

        public async Task AssignProductToCategoryAsync(ProductId productId, CategoryId categoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(productId));

            product.AssignCategory(categoryId);
            _productRepository.Update(product);
        }

        public async Task DeallocateProductFromCategoryAsync(ProductId productId, CategoryId categoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(productId));

            product.DeallocateCategory(categoryId);
            _productRepository.Update(product);
        }

        public async Task DeallocateProductFromAllCategoriesAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdAsync(productId).UnwrapAsync(
                new ProductNotFoundException(productId));

            product.DeallocateAllCategories();
            _productRepository.Update(product);
        }
    }
}