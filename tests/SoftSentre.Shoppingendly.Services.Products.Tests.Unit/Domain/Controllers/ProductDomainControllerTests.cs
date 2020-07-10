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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers;
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
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Controllers
{
    public class ProductDomainControllerTests : IAsyncLifetime
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICreatorBusinessRulesChecker> _creatorBusinessRulesCheckerMock;
        private Mock<ICategoryBusinessRulesChecker> _categoryBusinessRulesCheckMock;
        private Mock<IProductBusinessRulesChecker> _productBusinessRulesCheckerMock;
        private Mock<IProductBusinessRulesChecker> _productBusinessRulesCheckerForFactoryMock;
        private Mock<IDomainEventEmitter> _domainEventEmitterMock;
        private ProductFactory _productFactory;
        private Product _product;
        private ProductId _productId;
        private CreatorId _creatorId;
        private CategoryId _categoryId;
        private string _productName;
        private string _newProductName;
        private ProductProducer _productProducer;
        private ProductProducer _newProductProducer;
        private Picture _productPicture;
        private List<CategoryId> _productCategories;

        public async Task InitializeAsync()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _creatorBusinessRulesCheckerMock = new Mock<ICreatorBusinessRulesChecker>();
            _categoryBusinessRulesCheckMock = new Mock<ICategoryBusinessRulesChecker>();
            _productBusinessRulesCheckerMock = new Mock<IProductBusinessRulesChecker>();
            _productBusinessRulesCheckerForFactoryMock = new Mock<IProductBusinessRulesChecker>();
            _domainEventEmitterMock = new Mock<IDomainEventEmitter>();
            _productFactory = new ProductFactory(_productBusinessRulesCheckerForFactoryMock.Object,
                _creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);
            _productId = new ProductId(new Guid("45453C55-F12D-479E-832F-801A884677E0"));
            _creatorId = new CreatorId(new Guid("8AACD844-C6B8-46D4-B6AC-FE176D82FF9C"));
            _categoryId = new CategoryId(new Guid("9928BCC5-D362-49E2-9F94-AFC9AB8F7A7A"));
            _productName = "exampleProductName";
            _newProductName = "otherExampleProductName";
            _productProducer = ProductProducer.Create("exampleProductProducer");
            _newProductProducer = ProductProducer.Create("newExampleProductProducer");
            _productPicture = Picture.Create("exampleProductPictureName", "exampleProductPictureUrl");
            _product = _productFactory.Create(_productId, _creatorId, _productName, _productProducer);
            _productCategories = new List<CategoryId>()
            {
                new CategoryId(new Guid("892FEB4B-36FD-4C2B-B7C3-BC2FA2E9F7AB")),
                new CategoryId(new Guid("1A21A6F8-DEEF-4A33-897A-9A6F2FE226DF"))
            };

            await Task.CompletedTask;
        }

        [Fact]
        public async Task GetProductShouldReturnCorrectResult()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory,
                _domainEventEmitterMock.Object);

            // Act
            var product = await productDomainController.GetProductAsync(_productId);

            // Assert
            product.Should().Be(_product);
            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
        }

        [Fact]
        public async Task GetProductWithCategoriesShouldReturnCorrectResult()
        {
            // Arrange
            _product.AssignCategory(new CategoryId(new Guid("8EC75FC5-9B87-4072-AFAB-760188D9B195")));
            _product.AssignCategory(new CategoryId(new Guid("AE9D0FF4-72FA-4302-B333-1556835C3F0A")));
            _productRepositoryMock.Setup(pr => pr.GetByIdWithIncludesAsync(_product.ProductId)).ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var productWithCategoriesAsync =
                await productDomainController.GetProductWithCategoriesAsync(_product.ProductId);

            // Assert
            productWithCategoriesAsync.Should().Be(_product);
            productWithCategoriesAsync.Value.ProductCategories.Should().HaveCount(2);
            _productRepositoryMock.Verify(pr => pr.GetByIdWithIncludesAsync(_product.ProductId), Times.Once);
        }

        [Fact]
        public async Task GetAssignedCategoriesShouldReturnCorrectResult()
        {
            // Arrange
            _product.AssignCategory(new CategoryId(new Guid("8EC75FC5-9B87-4072-AFAB-760188D9B195")));
            _product.AssignCategory(new CategoryId(new Guid("AE9D0FF4-72FA-4302-B333-1556835C3F0A")));
            _productRepositoryMock.Setup(pr => pr.GetByIdWithIncludesAsync(_product.ProductId)).ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var productsCategories = await productDomainController.GetAssignedCategoriesAsync(_product.ProductId);

            //Assert
            productsCategories.Value.Should().HaveCount(2);
            _productRepositoryMock.Verify(pr => pr.GetByIdWithIncludesAsync(_productId), Times.Once);
        }

        [Fact]
        public async Task ProductShouldBeSuccessfullyAddedWhenParametersAreCorrect()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId));
            _productRepositoryMock.Setup(pr => pr.AddAsync(It.IsAny<Product>()));
            _productBusinessRulesCheckerMock
                .Setup(pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(null))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var product = await productDomainController.AddNewProductAsync(_product.ProductId, _product.CreatorId,
                _product.ProductName, _product.ProductProducer);

            //Assert
            product.Value.ProductId.Should().Be(_product.ProductId);
            product.Value.CreatorId.Should().Be(_product.CreatorId);
            product.Value.ProductName.Should().Be(_product.ProductName);
            product.Value.ProductProducer.Should().Be(_product.ProductProducer);
            product.Value.ProductPicture.Should().Be(Picture.Empty);
            product.Value.CreatedAt.Should().NotBe(default);

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task ProductWithPictureShouldBeSuccessfullyAddedWhenParametersAreCorrect()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId));
            _productBusinessRulesCheckerMock
                .Setup(pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture))
                .Returns(false);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var product = await productDomainController.AddNewProductAsync(_product.ProductId, _product.CreatorId,
                _product.ProductName, _product.ProductProducer, _productPicture);

            //Assert
            product.Value.ProductId.Should().Be(_product.ProductId);
            product.Value.CreatorId.Should().Be(_product.CreatorId);
            product.Value.ProductName.Should().Be(_product.ProductName);
            product.Value.ProductProducer.Should().Be(_product.ProductProducer);
            product.Value.ProductPicture.Should().Be(_productPicture);
            product.Value.CreatedAt.Should().NotBe(default);

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task ProductWithCategoriesShouldBeSuccessfullyAddedWhenParametersAreCorrect()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId));
            _productBusinessRulesCheckerMock
                .Setup(pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(null))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var product = await productDomainController.AddNewProductAsync(_product.ProductId, _product.CreatorId,
                _product.ProductName, _product.ProductProducer, _productCategories);

            //Assert
            product.Value.ProductId.Should().Be(_product.ProductId);
            product.Value.CreatorId.Should().Be(_product.CreatorId);
            product.Value.ProductName.Should().Be(_product.ProductName);
            product.Value.ProductProducer.Should().Be(_product.ProductProducer);
            product.Value.ProductPicture.Should().Be(_product.ProductPicture);
            product.Value.CreatedAt.Should().NotBe(default);
            product.Value.ProductCategories.Select(pr => pr.CategoryId).Should().BeEquivalentTo(_productCategories);

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task ProductWithPictureAndCategoriesShouldBeSuccessfullyAddedWhenParametersAreCorrect()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId));
            _productBusinessRulesCheckerMock
                .Setup(pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture))
                .Returns(false);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var product = await productDomainController.AddNewProductAsync(_product.ProductId, _product.CreatorId,
                _product.ProductName, _product.ProductProducer, _productPicture, _productCategories);

            //Assert
            product.Value.ProductId.Should().Be(_product.ProductId);
            product.Value.CreatorId.Should().Be(_product.CreatorId);
            product.Value.ProductName.Should().Be(_product.ProductName);
            product.Value.ProductProducer.Should().Be(_product.ProductProducer);
            product.Value.ProductPicture.Should().Be(_productPicture);
            product.Value.CreatedAt.Should().NotBe(default);
            product.Value.ProductCategories.Select(pr => pr.CategoryId).Should().BeEquivalentTo(_productCategories);

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void AddProductShouldBeFailedWhenCreatorAlreadyExists()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<Maybe<Product>>> addNewProduct = async () => await productDomainController.AddNewProductAsync(
                _productId,
                _product.CreatorId, _product.ProductName, _product.ProductProducer);

            //Assert
            addNewProduct.Should().Throw<ProductAlreadyExistsException>()
                .Where(e => e.Code == ErrorCodes.ProductAlreadyExists)
                .WithMessage($"Unable to add new product, because product with id: {_productId} is already exists.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.AddAsync(_product), Times.Never);
        }

        [Fact]
        public async Task ProductPictureShouldBeSuccessfullyUploadedWhenParametersAreCorrect()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock
                .Setup(pr => pr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture))
                .Returns(false);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var success = await productDomainController.UploadProductPictureAsync(_productId, _productPicture);

            //Assert
            success.Should().BeTrue();
            _product.ProductPicture.Should().Be(_productPicture);
            _product.ProductPicture.IsEmpty.Should().BeFalse();

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Once);

            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture), Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductPictureUploadedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductPicture.Equals(_product.ProductPicture))),
                Times.Once);
        }

        [Fact]
        public void UploadProductPictureShouldBeFailedWhenProductNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> uploadProductPicture = async () =>
                await productDomainController.UploadProductPictureAsync(_productId, _productPicture);

            //Assert
            uploadProductPicture.Should().Throw<ProductNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductNotFound)
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} not found.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture), Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductPictureUploadedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductPicture.Equals(_product.ProductPicture))),
                Times.Never);
        }

        [Fact]
        public void UploadProductPictureShouldBeFailedWhenProductIdHasBeenEmpty()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock
                .Setup(pr => pr.ProductIdCanNotBeEmptyRuleIsBroken(_productId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> uploadProductPicture = async () =>
                await productDomainController.UploadProductPictureAsync(_productId, Picture.Empty);

            //Assert
            uploadProductPicture.Should().Throw<InvalidProductIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidProductId)
                .WithMessage($"Invalid product id. {_productId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(Picture.Empty), Times.Never);
            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductPictureUploadedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductPicture.Equals(_product.ProductPicture))),
                Times.Never);
        }

        [Fact]
        public void UploadProductPictureShouldBeFailedWhenProductPictureHasBeenEmpty()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock
                .Setup(pr => pr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(Picture.Empty))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> uploadProductPicture = async () =>
                await productDomainController.UploadProductPictureAsync(_productId, Picture.Empty);

            //Assert
            uploadProductPicture.Should().Throw<ProductPictureCanNotBeNullOrEmptyException>()
                .Where(e => e.Code == ErrorCodes.ProductPictureCanNotBeNullOrEmpty)
                .WithMessage("Product picture can not be null or empty.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);
            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(Picture.Empty), Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductPictureUploadedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductPicture.Equals(_product.ProductPicture))),
                Times.Never);
        }

        [Fact]
        public async Task UploadedNewProductPictureIsTheSameAsExistingThenProductShouldNotBeUpdated()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var success = await productDomainController.UploadProductPictureAsync(_productId, _product.ProductPicture);

            //Assert
            success.Should().BeFalse();
            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_product.ProductPicture),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductPictureUploadedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductPicture.Equals(_product.ProductPicture))),
                Times.Never);
        }

        [Fact]
        public async Task ProductNameShouldBeSuccessfullyChangedWhenParametersAreCorrect()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var success = await productDomainController.ChangeProductNameAsync(_productId, _newProductName);

            //Assert
            success.Should().BeTrue();
            _product.ProductName.Should().Be(_newProductName);

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Once);

            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeLongerThanRuleIsBroken(_newProductName),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductNameChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductName == _newProductName)), Times.Once);
        }

        [Fact]
        public void ChangeProductNameShouldBeFailedWhenProductNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> changeProductName = async () =>
                await productDomainController.ChangeProductNameAsync(_productId, _newProductName);

            //Assert
            changeProductName.Should().Throw<ProductNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductNotFound)
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} not found.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeLongerThanRuleIsBroken(_newProductName),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductNameChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductName == _newProductName)), Times.Never);
        }

        [Fact]
        public void ChangeProductNameShouldBeFailedWhenProductIdIsNullOrEmpty()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock.Setup(pr => pr.ProductIdCanNotBeEmptyRuleIsBroken(_productId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> changeProductName = async () =>
                await productDomainController.ChangeProductNameAsync(_productId, _newProductName);

            //Assert
            changeProductName.Should().Throw<InvalidProductIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidProductId)
                .WithMessage($"Invalid product id. {_productId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_newProductName),
                Times.Never);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_newProductName),
                Times.Never);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeLongerThanRuleIsBroken(_newProductName),
                Times.Never);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductNameChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductName == _newProductName)), Times.Never);
        }

        [Fact]
        public void ChangeProductNameShouldBeFailedWhenProductNameAreEmpty()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock.Setup(pr => pr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_newProductName))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> changeProductName = async () =>
                await productDomainController.ChangeProductNameAsync(_productId, _newProductName);

            //Assert
            changeProductName.Should().Throw<ProductNameCanNotBeEmptyException>()
                .Where(e => e.Code == ErrorCodes.ProductNameCanNotBeEmpty)
                .WithMessage("Product name can not be empty.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_newProductName),
                Times.Never);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeLongerThanRuleIsBroken(_newProductName),
                Times.Never);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductNameChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductName == _newProductName)), Times.Never);
        }

        [Fact]
        public void ChangeProductNameShouldBeFailedWhenProductNameAreShorterThan()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock.Setup(pr => pr.ProductNameCanNotBeShorterThanRuleIsBroken(_newProductName))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> changeProductName = async () =>
                await productDomainController.ChangeProductNameAsync(_productId, _newProductName);

            //Assert
            changeProductName.Should().Throw<ProductNameIsTooShortException>()
                .Where(e => e.Code == ErrorCodes.ProductNameIsTooShort)
                .WithMessage(
                    $"Product name can not be shorter than {GlobalValidationVariables.ProductNameMinLength} characters.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeLongerThanRuleIsBroken(_newProductName),
                Times.Never);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductNameChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductName == _newProductName)), Times.Never);
        }

        [Fact]
        public void ChangeProductNameShouldBeFailedWhenProductNameAreLongerThan()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock.Setup(pr => pr.ProductNameCanNotBeLongerThanRuleIsBroken(_newProductName))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> changeProductName = async () =>
                await productDomainController.ChangeProductNameAsync(_productId, _newProductName);

            //Assert
            changeProductName.Should().Throw<ProductNameIsTooLongException>()
                .Where(e => e.Code == ErrorCodes.ProductNameIsTooLong)
                .WithMessage(
                    $"Product name can not be longer than {GlobalValidationVariables.ProductNameMaxLength} characters.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_newProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeLongerThanRuleIsBroken(_newProductName),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductNameChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductName == _newProductName)), Times.Never);
        }

        [Fact]
        public async Task WhenNewProductNameIsTheSameAsExistingThenProductShouldNotBeUpdated()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var success = await productDomainController.ChangeProductNameAsync(_productId, _product.ProductName);

            //Assert
            success.Should().BeFalse();
            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_product.ProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeShorterThanRuleIsBroken(_product.ProductName),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductNameCanNotBeLongerThanRuleIsBroken(_product.ProductName),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductNameChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductName == _product.ProductName)),
                Times.Never);
        }

        [Fact]
        public async Task ProductProducerShouldBeSuccessfullyChangedWhenParametersAreCorrect()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var success = await productDomainController.ChangeProductProducerAsync(_productId, _newProductProducer);

            //Assert
            success.Should().BeTrue();
            _product.ProductProducer.Should().Be(_newProductProducer);

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Once);


            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductProducerCanNotBeNullRuleIsBroken(_newProductProducer),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductProducerChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductProducer == _newProductProducer)),
                Times.Once);
        }

        [Fact]
        public void ChangeProductProducerShouldBeFailedWhenProductNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> changeProductProducer = async () =>
                await productDomainController.ChangeProductProducerAsync(_productId, _newProductProducer);

            //Assert
            changeProductProducer.Should().Throw<ProductNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductNotFound)
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} not found.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductProducerCanNotBeNullRuleIsBroken(_newProductProducer),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductProducerChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductProducer == _newProductProducer)),
                Times.Never);
        }

        [Fact]
        public void ChangeProductProducerShouldBeFailedWhenProductIdIsInvalid()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock
                .Setup(pr => pr.ProductIdCanNotBeEmptyRuleIsBroken(_productId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> changeProductName = async () =>
                await productDomainController.ChangeProductProducerAsync(_productId, _newProductProducer);

            //Assert
            changeProductName.Should().Throw<InvalidProductIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidProductId)
                .WithMessage($"Invalid product id. {_productId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductProducerChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductProducer == _newProductProducer)),
                Times.Never);
        }

        [Fact]
        public void ChangeProductProducerShouldBeFailedWhenProductProducerAreEmpty()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock
                .Setup(pr => pr.ProductProducerCanNotBeNullRuleIsBroken(_newProductProducer))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task<bool>> changeProductName = async () =>
                await productDomainController.ChangeProductProducerAsync(_productId, _newProductProducer);

            //Assert
            changeProductName.Should().Throw<ProductProducerCanNotBeNullException>()
                .Where(e => e.Code == ErrorCodes.ProductProducerCanNotBeNull)
                .WithMessage("Product producer can not be null.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductProducerCanNotBeNullRuleIsBroken(_newProductProducer),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductProducerChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductProducer == _newProductProducer)),
                Times.Never);
        }

        [Fact]
        public async Task WhenNewProductProducerIsTheSameAsExistingThenProductShouldNotBeUpdated()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock
                .Setup(pr => pr.ProductProducerCanNotBeNullRuleIsBroken(_product.ProductProducer))
                .Returns(false);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            var success =
                await productDomainController.ChangeProductProducerAsync(_productId, _product.ProductProducer);

            //Assert
            success.Should().BeFalse();
            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _productBusinessRulesCheckerMock.Verify(
                pbr => pbr.ProductProducerCanNotBeNullRuleIsBroken(_product.ProductProducer),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductProducerChangedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.ProductProducer == _product.ProductProducer)),
                Times.Never);
        }

        [Fact]
        public async Task CategoryShouldBeSuccessfullyAssigned()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            await productDomainController.AssignProductToCategoryAsync(_productId, _categoryId);

            //Assert
            _product.ProductCategories.Should().NotBeEmpty();
            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Once);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductAssignedToCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Once);
        }
        
        [Fact]
        public void AssignedCategoryShouldBeFailedWhenProductNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> changeProductProducer = async () =>
                await productDomainController.AssignProductToCategoryAsync(_productId, _categoryId);

            //Assert
            changeProductProducer.Should().Throw<ProductNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductNotFound)
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} not found.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductAssignedToCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Never);
        }
        
        [Fact]
        public void AssignedCategoryShouldBeFailedWhenProductIsAlreadyAssigned()
        {
            // Arrange
            _product.ProductCategories.Add(ProductCategory.Create(_productId, _categoryId));
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> changeProductProducer = async () =>
                await productDomainController.AssignProductToCategoryAsync(_productId, _categoryId);

            //Assert
            changeProductProducer.Should().Throw<ProductIsAlreadyAssignedToCategoryException>()
                .Where(e => e.Code == ErrorCodes.ProductIsAlreadyAssignedToCategory)
                .WithMessage($"Product already assigned to category with id: {_categoryId}.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductAssignedToCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Never);
        }

        [Fact]
        public void AssignedCategoryShouldBeFailedWhenProductIdIsNull()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock.Setup(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> assignProductToCategory = async () =>
                await productDomainController.AssignProductToCategoryAsync(_productId, _categoryId);

            //Assert
            assignProductToCategory.Should().Throw<InvalidProductIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidProductId)
                .WithMessage($"Invalid product id. {_productId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Never);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductAssignedToCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Never);
        }

        [Fact]
        public void AssignedCategoryShouldBeFailedWhenCategoryIdIsNull()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _categoryBusinessRulesCheckMock.Setup(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> assignProductToCategory = async () =>
                await productDomainController.AssignProductToCategoryAsync(_productId, _categoryId);

            //Assert
            assignProductToCategory.Should().Throw<InvalidCategoryIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidCategoryId)
                .WithMessage($"Invalid category id. {_categoryId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductAssignedToCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Never);
        }

        [Fact]
        public async Task ProductShouldBeSuccessfullyDeallocatedFromProduct()
        {
            // Arrange
            _product.ProductCategories.Add(ProductCategory.Create(_product.ProductId, _categoryId));
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            await productDomainController.DeallocateProductFromCategoryAsync(_productId, _categoryId);

            //Assert
            _product.ProductCategories.Should().BeEmpty();
            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Once);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Once);
        }
        
        [Fact]
        public void DeallocateProductFromCategoryShouldBeFailedWhenProductNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> changeProductProducer = async () =>
                await productDomainController.DeallocateProductFromCategoryAsync(_productId, _categoryId);

            //Assert
            changeProductProducer.Should().Throw<ProductNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductNotFound)
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} not found.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Never);
        }

        [Fact]
        public void DeallocateProductFromCategoryShouldBeFailedWhenProductIdIsNull()
        {
            // Arrange
            _product.ProductCategories.Add(ProductCategory.Create(_product.ProductId, _categoryId));
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock.Setup(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> deallocateProductFromCategory = async () =>
                await productDomainController.DeallocateProductFromCategoryAsync(_productId, _categoryId);

            //Assert
            deallocateProductFromCategory.Should().Throw<InvalidProductIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidProductId)
                .WithMessage($"Invalid product id. {_productId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Never);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Never);
        }

        [Fact]
        public void DeallocateProductFromCategoryShouldBeFailedWhenCategoryIdIsNull()
        {
            // Arrange
            _product.ProductCategories.Add(ProductCategory.Create(_product.ProductId, _categoryId));
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _categoryBusinessRulesCheckMock.Setup(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> deallocateProductFromCategory = async () =>
                await productDomainController.DeallocateProductFromCategoryAsync(_productId, _categoryId);

            //Assert
            deallocateProductFromCategory.Should().Throw<InvalidCategoryIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidCategoryId)
                .WithMessage($"Invalid category id. {_categoryId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Never);
        }

        [Fact]
        public void DeallocateProductFromCategoryShouldBeFailedWhenNotFoundInCategoryList()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> deallocateProductFromCategory = async () =>
                await productDomainController.DeallocateProductFromCategoryAsync(_productId, _categoryId);

            //Assert
            deallocateProductFromCategory.Should().Throw<ProductWithAssignedCategoryNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductWithAssignedCategoryNotFound)
                .WithMessage($"Product with assigned category with id: {_categoryId} not found.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);
            _categoryBusinessRulesCheckMock.Verify(pbr => pbr.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromCategoryDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId) && de.CategoryId.Equals(_categoryId))), Times.Never);
        }

        [Fact]
        public async Task ProductShouldBeSuccessfullyDeallocatedFromAllCategories()
        {
            // Arrange
            _product.ProductCategories.Add(ProductCategory.Create(_product.ProductId, _categoryId));
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            await productDomainController.DeallocateProductFromAllCategoriesAsync(_productId);

            //Assert
            _product.ProductCategories.Should().BeEmpty();
            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Once);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromAllCategoriesDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId))), Times.Once);
        }
        
        [Fact]
        public void DeallocateProductFromCategoriesShouldBeFailedWhenProductNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> deallocateProductFromAllCategories = async () =>
                await productDomainController.DeallocateProductFromAllCategoriesAsync(_productId);

            //Assert
            deallocateProductFromAllCategories.Should().Throw<ProductNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductNotFound)
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} not found.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromAllCategoriesDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId))), Times.Never);
        }

        [Fact]
        public void DeallocateProductFromCategoriesShouldBeFailedWhenProductIdIsNull()
        {
            // Arrange
            _product.ProductCategories.Add(ProductCategory.Create(_product.ProductId, _categoryId));
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock.Setup(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> deallocateProductFromAllCategories = async () =>
                await productDomainController.DeallocateProductFromAllCategoriesAsync(_productId);

            //Assert
            deallocateProductFromAllCategories.Should().Throw<InvalidProductIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidProductId)
                .WithMessage($"Invalid product id. {_productId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromAllCategoriesDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId))), Times.Never);
        }

        [Fact]
        public void DeallocateProductFromAllCategoriesShouldBeFailedWhenProductNotFoundInCategoryList()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> deallocateProductFromAllCategories = async () =>
                await productDomainController.DeallocateProductFromAllCategoriesAsync(_productId);

            //Assert
            deallocateProductFromAllCategories.Should().Throw<ProductWithAssignedCategoriesNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductWithAssignedCategoriesNotFound)
                .WithMessage("Unable to find any product with assigned categories.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Update(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductDeallocatedFromAllCategoriesDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId))), Times.Never);
        }

        [Fact]
        public async Task ProductShouldBeRemoved()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            await productDomainController.RemoveProductAsync(_productId);

            //Assert
            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Delete(_product), Times.Once);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductRemovedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId))), Times.Once);
        }

        [Fact]
        public void RemoveProductShouldBeFailedWhenProductIdIsNull()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            _productBusinessRulesCheckerMock.Setup(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId))
                .Returns(true);

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> removeProduct = async () =>
                await productDomainController.RemoveProductAsync(_productId);

            //Assert
            removeProduct.Should().Throw<InvalidProductIdException>()
                .Where(e => e.Code == ErrorCodes.InvalidProductId)
                .WithMessage($"Invalid product id. {_productId}");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Never);
            _productRepositoryMock.Verify(pr => pr.Delete(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductRemovedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId))), Times.Never);
        }
        
        [Fact]
        public void RemoveProductShouldBeFailedWhenProductNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainController productDomainController = new ProductDomainController(
                _productBusinessRulesCheckerMock.Object, _categoryBusinessRulesCheckMock.Object,
                _productRepositoryMock.Object, _productFactory, _domainEventEmitterMock.Object);

            // Act
            Func<Task> removeProduct = async () =>
                await productDomainController.RemoveProductAsync(_productId);

            //Assert
            removeProduct.Should().Throw<ProductNotFoundException>()
                .Where(e => e.Code == ErrorCodes.ProductNotFound)
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} not found.");

            _productRepositoryMock.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            _productRepositoryMock.Verify(pr => pr.Delete(_product), Times.Never);

            _productBusinessRulesCheckerMock.Verify(pbr => pbr.ProductIdCanNotBeEmptyRuleIsBroken(_productId),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(_product,
                    It.Is<ProductRemovedDomainEvent>(de =>
                        de.ProductId.Equals(_product.ProductId))), Times.Never);
        }
        
        public async Task DisposeAsync()
        {
            _productRepositoryMock = null;
            _creatorBusinessRulesCheckerMock = null;
            _categoryBusinessRulesCheckMock = null;
            _productBusinessRulesCheckerForFactoryMock = null;
            _productBusinessRulesCheckerMock = null;
            _domainEventEmitterMock = null;
            _productFactory = null;
            _productId = null;
            _creatorId = null;
            _categoryId = null;
            _productName = null;
            _newProductName = null;
            _productProducer = null;
            _newProductProducer = null;
            _productPicture = null;
            _product = null;
            _productCategories = null;

            await Task.CompletedTask;
        }
    }
}