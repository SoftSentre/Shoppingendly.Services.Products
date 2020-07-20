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

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects.StronglyTypedIds;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Factories
{
    public class ProductFactoryTests : IAsyncLifetime
    {
        private Mock<IProductBusinessRulesChecker> _productBusinessRulesCheckerMock;
        private Mock<ICreatorBusinessRulesChecker> _creatorBusinessRulesCheckerMock;
        private Mock<IDomainEventEmitter> _domainEventEmitterMock;
        private ProductFactory _productFactory;

        private ProductId _productId;
        private CreatorId _creatorId;
        private string _productName;
        private Producer _productProducer;
        private Picture _productPicture;

        public async Task InitializeAsync()
        {
            _productBusinessRulesCheckerMock = new Mock<IProductBusinessRulesChecker>();
            _creatorBusinessRulesCheckerMock = new Mock<ICreatorBusinessRulesChecker>();
            _domainEventEmitterMock = new Mock<IDomainEventEmitter>();
            _productId = new ProductId(new Guid("E806DD5E-2BF3-44A0-8BDB-62D43200F3A8"));
            _creatorId = new CreatorId(new Guid("1A8EA4A3-B7E7-4FDF-B2AA-378D5EB245BC"));
            _productName = "exampleProductName";
            _productProducer = Producer.Create("exampleProducerName");
            _productPicture = Picture.Create("examplePictureName", "examplePictureUrl");

            await Task.CompletedTask;
        }

        private void FailWhenProductIdIsEmpty(ProductId productId, CreatorId creatorId, string productName,
            Producer productProducer, ValueObject productPicture = null)
        {
            FailToCreateProductWhenBusinessRuleHasBeenBroken(
                checker => checker.ProductIdCanNotBeEmptyRuleIsBroken(productId),
                new InvalidProductIdException(productId), productId, creatorId,
                productName, productProducer, productPicture);
        }

        private void FailWhenCreatorIdIsEmpty(ProductId productId, CreatorId creatorId, string productName,
            Producer productProducer, ValueObject productPicture = null)
        {
            // Act
            _creatorBusinessRulesCheckerMock.Setup(cbr => cbr.CreatorIdCanNotBeEmptyRuleIsBroken(It.IsAny<CreatorId>()))
                .Returns(true);
            _productFactory = new ProductFactory(_productBusinessRulesCheckerMock.Object,
                _creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Func<Product> createProduct = () =>
                ChooseMethodToCreateProduct(productId, creatorId, productName, productProducer, productPicture);

            // Assert
            createProduct.Should().Throw<InvalidCreatorIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidCreatorId)
                .WithMessage($"Invalid creator id. {_creatorId}");

            _creatorBusinessRulesCheckerMock.Verify(cbr => cbr.CreatorIdCanNotBeEmptyRuleIsBroken(_creatorId),
                Times.Once);
            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Product>(), It.IsAny<NewProductCreatedDomainEvent>()),
                Times.Never);
        }

        private void FailWhenProductNameIsEmpty(ProductId productId, CreatorId creatorId, string productName,
            Producer productProducer, ValueObject productPicture = null)
        {
            FailToCreateProductWhenBusinessRuleHasBeenBroken(
                checker => checker.ProductNameCanNotBeNullOrEmptyRuleIsBroken(productName),
                new ProductNameCanNotBeEmptyException(), productId, creatorId,
                productName, productProducer, productPicture);
        }

        private void FailWhenProductNameIsTooShort(ProductId productId, CreatorId creatorId, string productName,
            Producer productProducer, ValueObject productPicture = null)
        {
            FailToCreateProductWhenBusinessRuleHasBeenBroken(
                checker => checker.ProductNameCanNotBeShorterThanRuleIsBroken(productName),
                new ProductNameIsTooShortException(GlobalValidationVariables.ProductNameMinLength), productId,
                creatorId, productName, productProducer, productPicture);
        }

        private void FailWhenProductNameIsTooLong(ProductId productId, CreatorId creatorId, string productName,
            Producer productProducer, ValueObject productPicture = null)
        {
            FailToCreateProductWhenBusinessRuleHasBeenBroken(
                checker => checker.ProductNameCanNotBeLongerThanRuleIsBroken(productName),
                new ProductNameIsTooLongException(GlobalValidationVariables.ProductNameMaxLength), productId, creatorId,
                productName, productProducer, productPicture);
        }

        private void FailWhenProductProducerIsNull(ProductId productId, CreatorId creatorId, string productName,
            Producer productProducer, ValueObject productPicture = null)
        {
            FailToCreateProductWhenBusinessRuleHasBeenBroken(
                checker => checker.ProductProducerCanNotBeNullRuleIsBroken(productProducer),
                new ProductProducerCanNotBeNullException(), productId, creatorId,
                productName, productProducer, productPicture);
        }

        private void FailToCreateProductWhenBusinessRuleHasBeenBroken<T>(
            Expression<Func<IProductBusinessRulesChecker, bool>> brokenRule, T exception, ProductId productId,
            CreatorId creatorId, string productName, Producer productProducer, ValueObject productPicture = null)
            where T : DomainException
        {
            // Act
            _productBusinessRulesCheckerMock.Setup(brokenRule).Returns(true);
            _productFactory = new ProductFactory(_productBusinessRulesCheckerMock.Object,
                _creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Func<Product> createProduct = () =>
                ChooseMethodToCreateProduct(productId, creatorId, productName, productProducer, productPicture);

            // Assert
            createProduct.Should().Throw<T>()
                .Where(e => e.Code == exception.Code)
                .WithMessage(exception.Message);

            _productBusinessRulesCheckerMock.Verify(brokenRule, Times.Once);
            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Product>(), It.IsAny<NewProductCreatedDomainEvent>()),
                Times.Never);
        }

        private Product ChooseMethodToCreateProduct(ProductId productId, CreatorId creatorId, string productName,
            Producer productProducer, ValueObject productPicture = null)
        {
            var product = productPicture == null
                ? _productFactory.Create(productId, creatorId, productName, productProducer)
                : _productFactory.Create(productId, creatorId, _productPicture, productName, productProducer);

            return product;
        }

        public async Task DisposeAsync()
        {
            _productBusinessRulesCheckerMock = null;
            _creatorBusinessRulesCheckerMock = null;
            _domainEventEmitterMock = null;
            _productId = null;
            _creatorId = null;
            _productName = null;
            _productProducer = null;
            _productPicture = null;

            await Task.CompletedTask;
        }

        [Fact]
        public void CreateProductShouldNotRiseAnyExceptionWhenParametersAreCorrect()
        {
            // Act
            _productFactory = new ProductFactory(_productBusinessRulesCheckerMock.Object,
                _creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Func<Product> createProduct = () =>
                _productFactory.Create(_productId, _creatorId, _productName, _productProducer);

            // Assert
            createProduct.Should().NotThrow<DomainException>();
        }

        [Fact]
        public void CreateProductWithPictureShouldNotRiseAnyExceptionWhenParametersAreCorrect()
        {
            // Act
            _productFactory = new ProductFactory(_productBusinessRulesCheckerMock.Object,
                _creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Func<Product> createProduct = () =>
                _productFactory.Create(_productId, _creatorId, _productPicture, _productName, _productProducer);

            // Assert
            createProduct.Should().NotThrow<DomainException>();
        }

        [Fact]
        public void FailToCreateProductWhenCreatorIdIsEmpty()
        {
            FailWhenCreatorIdIsEmpty(_productId, _creatorId, _productName, _productProducer);
        }

        [Fact]
        public void FailToCreateProductWhenProductIdIsEmpty()
        {
            FailWhenProductIdIsEmpty(_productId, _creatorId, _productName, _productProducer);
        }

        [Fact]
        public void FailToCreateProductWhenProductNameIsEmpty()
        {
            FailWhenProductNameIsEmpty(_productId, _creatorId, _productName, _productProducer);
        }

        [Fact]
        public void FailToCreateProductWhenProductNameIsTooLong()
        {
            FailWhenProductNameIsTooLong(_productId, _creatorId, _productName, _productProducer);
        }

        [Fact]
        public void FailToCreateProductWhenProductNameIsTooShort()
        {
            FailWhenProductNameIsTooShort(_productId, _creatorId, _productName, _productProducer);
        }

        [Fact]
        public void FailToCreateProductWhenProductProducerIsNull()
        {
            FailWhenProductProducerIsNull(_productId, _creatorId, _productName, _productProducer);
        }

        [Fact]
        public void FailToCreateProductWithPictureWhenCreatorIdIsEmpty()
        {
            FailWhenCreatorIdIsEmpty(_productId, _creatorId, _productName, _productProducer, _productPicture);
        }

        [Fact]
        public void FailToCreateProductWithPictureWhenProductIdIsEmpty()
        {
            FailWhenProductIdIsEmpty(_productId, _creatorId, _productName, _productProducer, _productPicture);
        }

        [Fact]
        public void FailToCreateProductWithPictureWhenProductNameIsEmpty()
        {
            FailWhenProductNameIsEmpty(_productId, _creatorId, _productName, _productProducer, _productPicture);
        }

        [Fact]
        public void FailToCreateProductWithPictureWhenProductNameIsTooLong()
        {
            FailWhenProductNameIsTooLong(_productId, _creatorId, _productName, _productProducer, _productPicture);
        }

        [Fact]
        public void FailToCreateProductWithPictureWhenProductNameIsTooShort()
        {
            FailWhenProductNameIsTooShort(_productId, _creatorId, _productName, _productProducer, _productPicture);
        }

        [Fact]
        public void FailToCreateProductWithPictureWhenProductPictureIsNullOrEmpty()
        {
            // Act
            _productBusinessRulesCheckerMock
                .Setup(pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture))
                .Returns(true);

            _productFactory = new ProductFactory(_productBusinessRulesCheckerMock.Object,
                _creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Func<Product> createProduct = () =>
                _productFactory.Create(_productId, _creatorId, _productPicture, _productName, _productProducer);

            // Assert
            createProduct.Should().Throw<ProductPictureCanNotBeNullOrEmptyException>()
                .Where(e => e.Code == ErrorCodes.ProductPictureCanNotBeNullOrEmpty)
                .WithMessage("Product picture can not be null or empty.");

            _productBusinessRulesCheckerMock.Verify(
                cbr => cbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture),
                Times.Once);
            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Product>(), It.IsAny<NewProductCreatedDomainEvent>()),
                Times.Never);
        }

        [Fact]
        public void FailToCreateProductWithPictureWhenProductProducerIsNull()
        {
            FailWhenProductProducerIsNull(_productId, _creatorId, _productName, _productProducer, _productPicture);
        }

        [Fact]
        public void SuccessToCreateProductWhenParametersAreCorrect()
        {
            // Act
            _productFactory = new ProductFactory(_productBusinessRulesCheckerMock.Object,
                _creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            var product = _productFactory.Create(_productId, _creatorId, _productName, _productProducer);

            // Assert
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _creatorBusinessRulesCheckerMock.Verify(cbr => cbr.CreatorIdCanNotBeEmptyRuleIsBroken(_creatorId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_productName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_productName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_productName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductProducerCanNotBeNullRuleIsBroken(_productProducer), Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture), Times.Never);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Product>(),
                    It.Is<NewProductCreatedDomainEvent>(de =>
                        de.ProductId.Equals(product.ProductId) && de.CreatorId.Equals(product.CreatorId) &&
                        de.ProductName == product.ProductName && de.ProductProducer.Equals(product.ProductProducer) &&
                        de.ProductPicture.Equals(product.ProductPicture))), Times.Once);
        }

        [Fact]
        public void SuccessToCreateProductWithPictureWhenParametersAreCorrect()
        {
            // Act
            _productFactory = new ProductFactory(_productBusinessRulesCheckerMock.Object,
                _creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            var product =
                _productFactory.Create(_productId, _creatorId, _productPicture, _productName, _productProducer);

            // Assert
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _creatorBusinessRulesCheckerMock.Verify(cbr => cbr.CreatorIdCanNotBeEmptyRuleIsBroken(_creatorId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_productName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_productName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_productName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductProducerCanNotBeNullRuleIsBroken(_productProducer), Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture), Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Product>(),
                    It.Is<NewProductCreatedDomainEvent>(de =>
                        de.ProductId.Equals(product.ProductId) && de.CreatorId.Equals(product.CreatorId) &&
                        de.ProductName == product.ProductName && de.ProductProducer.Equals(product.ProductProducer) &&
                        de.ProductPicture.Equals(product.ProductPicture))), Times.Once);
        }
    }
}