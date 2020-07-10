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
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Globals;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Controllers
{
    public class ProductDomainController : IProductDomainController
    {
        private readonly IProductBusinessRulesChecker _productBusinessRulesChecker;
        private readonly ICategoryBusinessRulesChecker _categoryBusinessRulesChecker;
        private readonly IDomainEventEmitter _domainEventEmitter;
        private readonly IProductRepository _productRepository;
        private readonly ProductFactory _productFactory;

        public ProductDomainController(IProductBusinessRulesChecker productBusinessRulesChecker,
            ICategoryBusinessRulesChecker categoryBusinessRulesChecker, IProductRepository productRepository,
            ProductFactory productFactory, IDomainEventEmitter domainEventEmitter)
        {
            _productBusinessRulesChecker = productBusinessRulesChecker.IfEmptyThenThrowAndReturnValue();
            _categoryBusinessRulesChecker = categoryBusinessRulesChecker.IfEmptyThenThrowAndReturnValue();
            _domainEventEmitter = domainEventEmitter.IfEmptyThenThrowAndReturnValue();
            _productRepository = productRepository.IfEmptyThenThrowAndReturnValue();
            _productFactory = productFactory.IfEmptyThenThrowAndReturnValue();
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

        public async Task<Maybe<IEnumerable<ProductCategory>>> GetAssignedCategoriesAsync(ProductId productId)
        {
            var product = await _productRepository.GetByIdWithIncludesAsync(productId).UnwrapAsync(
                new ProductNotFoundException(productId));

            var assignedCategories = product.GetAllAssignedCategories();

            return assignedCategories;
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId,
            string productName, ProductProducer producer)
        {
            return await CreateProductAsync(productId, creatorId, productName, producer);
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId,
            string productName, ProductProducer producer, Picture productPicture)
        {
            return await CreateProductAsync(productId, creatorId, productName, producer, productPicture);
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId,
            string productName, ProductProducer producer, IEnumerable<CategoryId> categoryIds)
        {
            return await CreateProductAsync(productId, creatorId, productName, producer, categoryIds: categoryIds);
        }

        public async Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId,
            string productName, ProductProducer producer, Picture productPicture, IEnumerable<CategoryId> categoryIds)
        {
            return await CreateProductAsync(productId, creatorId, productName, producer, productPicture, categoryIds);
        }

        public async Task<bool> UploadProductPictureAsync(ProductId productId, Picture productPicture)
        {
            if (_productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(productId))
                throw new InvalidProductIdException(productId);
            if (_productBusinessRulesChecker.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(productPicture))
                throw new ProductPictureCanNotBeNullOrEmptyException();

            var product =
                await _productRepository.GetByIdAndThrowIfEntityNotFound(productId,
                    new ProductNotFoundException(productId));

            var isPictureChanged = product.UploadProductPicture(productPicture);

            if (isPictureChanged)
            {
                _domainEventEmitter.Emit(product, new ProductPictureUploadedDomainEvent(product.ProductId, product.ProductPicture));
                _productRepository.Update(product);
            }

            return isPictureChanged;
        }

        public async Task<bool> ChangeProductNameAsync(ProductId productId, string productName)
        {
            if (_productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(productId))
                throw new InvalidProductIdException(productId);
            if (_productBusinessRulesChecker.ProductNameCanNotBeNullOrEmptyRuleIsBroken(productName))
                throw new ProductNameCanNotBeEmptyException();
            if (_productBusinessRulesChecker.ProductNameCanNotBeShorterThanRuleIsBroken(productName))
                throw new ProductNameIsTooShortException(GlobalValidationVariables.ProductNameMinLength);
            if (_productBusinessRulesChecker.ProductNameCanNotBeLongerThanRuleIsBroken(productName))
                throw new ProductNameIsTooLongException(GlobalValidationVariables.ProductNameMaxLength);

            var product =
                await _productRepository.GetByIdAndThrowIfEntityNotFound(productId,
                    new ProductNotFoundException(productId));

            var isNameChanged = product.ChangeProductName(productName);

            if (isNameChanged)
            {
                _domainEventEmitter.Emit(product, new ProductNameChangedDomainEvent(product.ProductId, product.ProductName));
                _productRepository.Update(product);
            }
            
            return isNameChanged;
        }

        public async Task<bool> ChangeProductProducerAsync(ProductId productId, ProductProducer productProducer)
        {
            if (_productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(productId))
                throw new InvalidProductIdException(productId);
            if (_productBusinessRulesChecker.ProductProducerCanNotBeNullRuleIsBroken(productProducer))
                throw new ProductProducerCanNotBeNullException();

            var product =
                await _productRepository.GetByIdAndThrowIfEntityNotFound(productId,
                    new ProductNotFoundException(productId));

            var isProducerChanged = product.ChangeProductProducer(productProducer);

            if (isProducerChanged)
            {
                _domainEventEmitter.Emit(product, new ProductProducerChangedDomainEvent(product.ProductId, product.ProductProducer));
                _productRepository.Update(product);
            }
            
            return isProducerChanged;
        }

        public async Task AssignProductToCategoryAsync(ProductId productId, CategoryId categoryId)
        {
            if (_productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(productId))
                throw new InvalidProductIdException(productId);
            if (_categoryBusinessRulesChecker.CategoryIdCanNotBeEmptyRuleIsBroken(categoryId))
                throw new InvalidCategoryIdException(categoryId);

            var product =
                await _productRepository.GetByIdAndThrowIfEntityNotFound(productId,
                    new ProductNotFoundException(productId));

            product.AssignCategory(categoryId);
            _domainEventEmitter.Emit(product, new ProductAssignedToCategoryDomainEvent(product.ProductId, categoryId));
            _productRepository.Update(product);
        }

        public async Task DeallocateProductFromCategoryAsync(ProductId productId, CategoryId categoryId)
        {
            if (_productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(productId))
                throw new InvalidProductIdException(productId);
            if (_categoryBusinessRulesChecker.CategoryIdCanNotBeEmptyRuleIsBroken(categoryId))
                throw new InvalidCategoryIdException(categoryId);

            var product =
                await _productRepository.GetByIdAndThrowIfEntityNotFound(productId,
                    new ProductNotFoundException(productId));

            product.DeallocateCategory(categoryId);
            _domainEventEmitter.Emit(product,
                new ProductDeallocatedFromCategoryDomainEvent(product.ProductId, categoryId));
            _productRepository.Update(product);
        }

        public async Task DeallocateProductFromAllCategoriesAsync(ProductId productId)
        {
            if (_productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(productId))
                throw new InvalidProductIdException(productId);

            var product =
                await _productRepository.GetByIdAndThrowIfEntityNotFound(productId,
                    new ProductNotFoundException(productId));

            product.DeallocateAllCategories();
            _domainEventEmitter.Emit(product, new ProductDeallocatedFromAllCategoriesDomainEvent(product.ProductId));
            _productRepository.Update(product);
        }

        public async Task RemoveProductAsync(ProductId productId)
        {
            if (_productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(productId))
                throw new InvalidProductIdException(productId);
            
            var product =
                await _productRepository.GetByIdAndThrowIfEntityNotFound(productId,
                    new ProductNotFoundException(productId));
            
            _domainEventEmitter.Emit(product, new ProductRemovedDomainEvent(product.ProductId));
            _productRepository.Delete(product);
        }

        private async Task<Product> CreateProductAsync(ProductId productId, CreatorId creatorId, string productName,
            ProductProducer producer, Picture productPicture = null, IEnumerable<CategoryId> categoryIds = null)
        {
            await _productRepository.GetByIdAndThrowIfEntityAlreadyExists(productId,
                new ProductAlreadyExistsException(productId));

            var newProduct = _productBusinessRulesChecker.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(productPicture)
                ? _productFactory.Create(productId, creatorId, productName, producer)
                : _productFactory.Create(productId, creatorId, productPicture, productName, producer);

            if (categoryIds != null)
            {
                var categoryIdsAsList = categoryIds.ToList();

                if (categoryIdsAsList.Any())
                {
                    categoryIdsAsList.ForEach(ci => newProduct.AssignCategory(ci));
                }
            }

            await _productRepository.AddAsync(newProduct);
            return newProduct;
        }
    }
}