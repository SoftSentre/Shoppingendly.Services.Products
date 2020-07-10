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

using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Globals;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Factories
{
    public class ProductFactory
    {
        private readonly IProductBusinessRulesChecker _productBusinessRulesChecker;
        private readonly ICreatorBusinessRulesChecker _creatorBusinessRulesChecker;
        private readonly IDomainEventEmitter _domainEventEmitter;

        internal ProductFactory(IProductBusinessRulesChecker productBusinessRulesChecker,
            ICreatorBusinessRulesChecker creatorBusinessRulesChecker, IDomainEventEmitter domainEventEmitter)
        {
            _productBusinessRulesChecker = productBusinessRulesChecker.IfEmptyThenThrowAndReturnValue();
            _creatorBusinessRulesChecker = creatorBusinessRulesChecker.IfEmptyThenThrowAndReturnValue();
            _domainEventEmitter = domainEventEmitter.IfEmptyThenThrowAndReturnValue();
        }

        internal Product Create(ProductId productId, CreatorId creatorId, string productName,
            ProductProducer productProducer)
        {
            CheckBusinessRules(productId, creatorId, productName, productProducer);

            var product = new Product(productId, creatorId, productName, productProducer);
            _domainEventEmitter.Emit(product, new NewProductCreatedDomainEvent(productId, creatorId, productName,
                productProducer, Picture.Empty));

            return product;
        }

        internal Product Create(ProductId productId, CreatorId creatorId, Picture productPicture,
            string productName, ProductProducer productProducer)
        {
            CheckBusinessRules(productId, creatorId, productName, productProducer, productPicture);

            var product = new Product(productId, creatorId, productPicture, productName, productProducer);
            _domainEventEmitter.Emit(product, new NewProductCreatedDomainEvent(productId, creatorId, productName,
                productProducer, productPicture));

            return product;
        }

        private void CheckBusinessRules(ProductId productId, CreatorId creatorId,
            string productName, ProductProducer productProducer, Picture productPicture = null)
        {
            if (_productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(productId))
                throw new InvalidProductIdException(productId);
            if (_creatorBusinessRulesChecker.CreatorIdCanNotBeEmptyRuleIsBroken(creatorId))
                throw new InvalidCreatorIdException(creatorId);
            if (_productBusinessRulesChecker.ProductNameCanNotBeNullOrEmptyRuleIsBroken(productName))
                throw new ProductNameCanNotBeEmptyException();
            if (_productBusinessRulesChecker.ProductNameCanNotBeShorterThanRuleIsBroken(productName))
                throw new ProductNameIsTooShortException(GlobalValidationVariables.ProductNameMinLength);
            if (_productBusinessRulesChecker.ProductNameCanNotBeLongerThanRuleIsBroken(productName))
                throw new ProductNameIsTooLongException(GlobalValidationVariables.ProductNameMaxLength);
            if (_productBusinessRulesChecker.ProductProducerCanNotBeNullRuleIsBroken(productProducer))
                throw new ProductProducerCanNotBeNullException();
            if (productPicture != null &&
                _productBusinessRulesChecker.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(productPicture))
                throw new ProductPictureCanNotBeNullOrEmptyException();
        }
    }
}