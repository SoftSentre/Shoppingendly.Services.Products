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
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Services
{
    public class ProductDomainServiceTests
    {
        public ProductDomainServiceTests()
        {
            _productId = new ProductId();
            _productProducer = ProductProducer.CreateProductProducer("ExampleProducer");
            _productPicture = ProductPicture.Create("PictureName", "PictureUrl");
            _product = Product.Create(_productId, new CreatorId(), ProductName, _productProducer);
        }

        private const string ProductName = "ExampleProductName";

        private readonly ProductProducer _productProducer;
        private readonly ProductId _productId;
        private readonly ProductPicture _productPicture;
        private readonly Product _product;

        [Fact]
        public void CheckIfAddNewProductMethodDoNotThrowException()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task<Maybe<Product>>> func = async () => await productDomainService.AddNewProductAsync(_productId,
                _product.CreatorId, _product.ProductName, _product.Producer);

            //Assert
            func.Should().NotThrow();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void CheckIfAddNewProductMethodThrowExceptionWhenCreatorAlreadyExists()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task<Maybe<Product>>> func = async () => await productDomainService.AddNewProductAsync(_productId,
                _product.CreatorId, _product.ProductName, _product.Producer);

            //Assert
            func.Should().Throw<ProductAlreadyExistsException>()
                .WithMessage($"Unable to add new product, because product with id: {_productId} is already exists.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CheckIfAddNewProductMethodWithCategoriesCreateValidObject()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(_productId, new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId));
            productRepository.Setup(pr => pr.AddAsync(product));

            var firstAssignedCategory = new CategoryId();
            var secondAssignedCategory = new CategoryId();
            IEnumerable<CategoryId> categories = new[] {firstAssignedCategory, secondAssignedCategory};
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.AddNewProductAsync(product.Id, product.CreatorId,
                product.ProductName,
                product.Producer, categories);

            //Assert
            testResult.Value.Id.Should().Be(product.Id);
            testResult.Value.CreatorId.Should().Be(product.CreatorId);
            testResult.Value.ProductName.Should().Be(product.ProductName);
            testResult.Value.Producer.Should().Be(product.Producer);
            testResult.Value.CreatedAt.Should().NotBe(default);
            var expectedFirstCategory =
                testResult.Value.ProductCategories.FirstOrDefault(pr => pr.SecondKey.Equals(firstAssignedCategory)) ??
                It.IsAny<ProductCategory>();
            expectedFirstCategory.Should().NotBeNull();
            var expectedSecondCategory =
                testResult.Value.ProductCategories.FirstOrDefault(pr => pr.SecondKey.Equals(secondAssignedCategory)) ??
                It.IsAny<ProductCategory>();
            expectedSecondCategory.Should().NotBeNull();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task CheckIfAddNewProductMethodWithNoCategoriesCreateValidObject()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.AddNewProductAsync(_productId, _product.CreatorId,
                _product.ProductName, _product.Producer);

            //Assert
            testResult.Value.Id.Should().Be(_product.Id);
            testResult.Value.CreatorId.Should().Be(_product.CreatorId);
            testResult.Value.ProductName.Should().Be(_product.ProductName);
            testResult.Value.Producer.Should().Be(_product.Producer);
            testResult.Value.CreatedAt.Should().NotBe(default);
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void CheckIfAddNewProductWithCategoriesMethodDoNotThrowException()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task<Maybe<Product>>> func = async () => await productDomainService.AddNewProductAsync(_productId,
                _product.CreatorId, _product.ProductName, _product.Producer,
                new List<CategoryId> {new CategoryId(), new CategoryId()});

            //Assert
            func.Should().NotThrow();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void CheckIfAddNewProductWithCategoriesMethodThrowExceptionWhenCreatorAlreadyExists()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task<Maybe<Product>>> func = async () => await productDomainService.AddNewProductAsync(_productId,
                _product.CreatorId, _product.ProductName, _product.Producer,
                new List<CategoryId> {new CategoryId(), new CategoryId()});

            //Assert
            func.Should().Throw<ProductAlreadyExistsException>()
                .WithMessage($"Unable to add new product, because product with id: {_productId} is already exists.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.AddAsync(It.IsAny<Product>()), Times.Never);
        }


        [Fact]
        public async Task CheckIfAddOrChangeProductPictureMethodCreateValidObject()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.AddOrChangeProductPictureAsync(_productId, _productPicture);

            //Assert
            testResult.Should().BeTrue();
            product.ProductPicture.Should().Be(_productPicture);
            product.ProductPicture.IsEmpty.Should().BeFalse();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfAddOrChangeProductPictureMethodDoNotThrownWhenCorrectValuesAreProvided()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () =>
                await productDomainService.AddOrChangeProductPictureAsync(_productId, _productPicture);

            //Assert
            func.Should().NotThrow();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfAddOrChangeProductPictureMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();

            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () =>
                await productDomainService.AddOrChangeProductPictureAsync(_productId, ProductPicture.Empty);

            //Assert
            func.Should().Throw<ProductNotFoundException>()
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} is empty.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CheckIfAssignProductToCategoryMethodAddedOneElementToList()
        {
            // Arrange
            var categoryId = new CategoryId();
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            await productDomainService.AssignProductToCategoryAsync(_productId, categoryId);

            //Assert
            product.ProductCategories.Should().NotBeEmpty();
            var expectedValue = product.ProductCategories.FirstOrDefault() ?? It.IsAny<ProductCategory>();
            expectedValue.FirstKey.Should().Be(product.Id);
            expectedValue.SecondKey.Should().Be(categoryId);
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfAssignProductToCategoryMethodDoNotThrown()
        {
            // Arrange
            var categoryId = new CategoryId();
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () =>
                await productDomainService.AssignProductToCategoryAsync(_productId, categoryId);

            //Assert
            func.Should().NotThrow();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfAssignProductToCategoryMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var categoryId = new CategoryId();
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () =>
                await productDomainService.AssignProductToCategoryAsync(_productId, categoryId);

            //Assert
            func.Should().Throw<ProductNotFoundException>()
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} is empty.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void CheckIfChangeProductNameMethodDoNotThrownAnyException()
        {
            // Arrange
            const string newProductName = "OtherExampleProductName";
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task<bool>> func = async () =>
                await productDomainService.ChangeProductNameAsync(_productId, newProductName);

            //Assert
            func.Should().NotThrow();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public async Task CheckIfChangeProductNameMethodReturnTrueAndSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newProductName = "OtherExampleProductName";
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.ChangeProductNameAsync(_productId, newProductName);

            //Assert
            testResult.Should().BeTrue();
            product.ProductName.Should().Be(newProductName);
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfChangeProductNameMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            const string newProductName = "OtherExampleProductName";
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task<bool>> func = async () =>
                await productDomainService.ChangeProductNameAsync(_productId, newProductName);

            //Assert
            func.Should().Throw<ProductNotFoundException>()
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} is empty.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void CheckIfChangeProductProducerMethodDoNotThrownAnyException()
        {
            // Arrange
            var newProductProducer = ProductProducer.CreateProductProducer("OtherExampleProductProducer");
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task<bool>> func = async () =>
                await productDomainService.ChangeProductProducerAsync(_productId, newProductProducer);

            //Assert
            func.Should().NotThrow();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }


        [Fact]
        public async Task CheckIfChangeProductProducerMethodReturnTrueAndSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            var newProductProducer = ProductProducer.CreateProductProducer("OtherExampleProductProducer");
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.ChangeProductProducerAsync(_productId, newProductProducer);

            //Assert
            testResult.Should().BeTrue();
            product.Producer.Should().Be(newProductProducer);
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfChangeProductProducerMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var newProductProducer = ProductProducer.CreateProductProducer("OtherExampleProductProducer");
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task<bool>> func = async () =>
                await productDomainService.ChangeProductProducerAsync(_productId, newProductProducer);

            //Assert
            func.Should().Throw<ProductNotFoundException>()
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} is empty.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CheckIfDeallocatedProductFromAllCategoriesMethodAddedOneElementToList()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            product.ProductCategories.Add(new ProductCategory(_productId, new CategoryId()));
            product.ProductCategories.Add(new ProductCategory(_productId, new CategoryId()));
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            await productDomainService.DeallocateProductFromAllCategoriesAsync(_productId);

            //Assert
            product.ProductCategories.Should().BeEmpty();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfDeallocatedProductFromAllCategoriesMethodDoNotThrown()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            product.ProductCategories.Add(new ProductCategory(_productId, new CategoryId()));
            product.ProductCategories.Add(new ProductCategory(_productId, new CategoryId()));
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () =>
                await productDomainService.DeallocateProductFromAllCategoriesAsync(_productId);

            //Assert
            func.Should().NotThrow();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public async Task CheckIfDeallocatedProductFromCategoryMethodAddedOneElementToList()
        {
            // Arrange
            var categoryId = new CategoryId();
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            product.ProductCategories.Add(new ProductCategory(_productId, categoryId));
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            await productDomainService.DeallocateProductFromCategoryAsync(_productId, categoryId);

            //Assert
            product.ProductCategories.Should().BeEmpty();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfDeallocatedProductToCategoryMethodDoNotThrown()
        {
            // Arrange
            var categoryId = new CategoryId();
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            product.ProductCategories.Add(new ProductCategory(_productId, categoryId));
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () =>
                await productDomainService.DeallocateProductFromCategoryAsync(_productId, categoryId);

            //Assert
            func.Should().NotThrow();
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }

        [Fact]
        public void CheckIfDeallocateProductFromAllCategoriesMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () =>
                await productDomainService.DeallocateProductFromAllCategoriesAsync(_productId);

            //Assert
            func.Should().Throw<ProductNotFoundException>()
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} is empty.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void CheckIfDeallocateProductFromCategoryMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var categoryId = new CategoryId();
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () =>
                await productDomainService.DeallocateProductFromCategoryAsync(_productId, categoryId);

            //Assert
            func.Should().Throw<ProductNotFoundException>()
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} is empty.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CheckIfGetAssignedCategoriesMethodReturnNoValueWhenProductHasNotAssignAnyCategory()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdWithIncludesAsync(_productId))
                .ReturnsAsync(_product);
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.GetAssignedCategoriesAsync(_productId);

            //Assert
            testResult.HasValue.Should().BeTrue();
            testResult.Value.Should().BeEmpty();
            productRepository.Verify(pr => pr.GetByIdWithIncludesAsync(_productId), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetAssignedCategoriesMethodReturnValueWhenProductHasAssignSomeCategory()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", _productProducer);
            product.ProductCategories.Add(ProductCategory.Create(product.Id, new CategoryId()));
            product.ProductCategories.Add(ProductCategory.Create(product.Id, new CategoryId()));
            productRepository.Setup(pr => pr.GetByIdWithIncludesAsync(_productId))
                .ReturnsAsync(product);
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.GetAssignedCategoriesAsync(_productId);

            //Assert
            testResult.HasValue.Should().BeTrue();
            testResult.Value.Should().HaveCount(2);
            productRepository.Verify(pr => pr.GetByIdWithIncludesAsync(_productId), Times.Once);
        }

        [Fact]
        public void CheckIfGetAssignedCategoriesMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdWithIncludesAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () => await productDomainService.GetAssignedCategoriesAsync(_productId);

            //Assert
            func.Should().Throw<ProductNotFoundException>()
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} is empty.");
            productRepository.Verify(pr => pr.GetByIdWithIncludesAsync(_productId), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetManyProductsWithCategoriesMethodReturnValidObject()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());
            var productList = new List<Product> {product};

            productRepository.Setup(cr => cr.GetManyByNameWithIncludesAsync(ProductName))
                .ReturnsAsync(productList);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.GetProductsByNameWithCategoriesAsync(ProductName);

            // Assert
            testResult.Should().Be(productList);
            var firstItem = testResult.Value.FirstOrDefault() ?? It.IsAny<Product>();
            firstItem.ProductCategories.Should().HaveCount(2);
            firstItem.Should().Be(product);
            productRepository.Verify(pr => pr.GetManyByNameWithIncludesAsync(ProductName), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetProductMethodReturnValidObject()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(_product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.GetProductAsync(_productId);

            // Assert
            testResult.Should().Be(_product);
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetProductsByNameMethodReturnValidObject()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var productList = new List<Product>
            {
                _product
            };

            productRepository.Setup(pr => pr.GetManyByNameAsync(ProductName))
                .ReturnsAsync(productList);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.GetProductsByNameAsync(ProductName);

            // Assert
            testResult.Should().Be(productList);
            productRepository.Verify(pr => pr.GetManyByNameAsync(ProductName), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetProductWithCategoriesMethodReturnValidObject()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), ProductName, _productProducer);
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());
            productRepository.Setup(pr => pr.GetByIdWithIncludesAsync(product.Id))
                .ReturnsAsync(product);
            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            var testResult = await productDomainService.GetProductWithCategoriesAsync(product.Id);

            // Assert
            testResult.Should().Be(product);
            testResult.Value.ProductCategories.Should().HaveCount(2);
            productRepository.Verify(pr => pr.GetByIdWithIncludesAsync(product.Id), Times.Once);
        }

        [Fact]
        public void CheckIfRemovePictureFromProductMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(new Maybe<Product>());

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            Func<Task> func = async () => await productDomainService.RemovePictureFromProductAsync(_productId);

            //Assert
            func.Should().Throw<ProductNotFoundException>()
                .WithMessage($"Unable to mutate product state, because product with id: {_productId} is empty.");
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CheckIfRemoveProductPictureMethodDoNotThrownWhenCorrectValuesAreProvided()
        {
            // Arrange
            var productRepository = new Mock<IProductRepository>();
            var product = new Product(new ProductId(), new CreatorId(), _productPicture, ProductName, _productProducer);
            productRepository.Setup(pr => pr.GetByIdAsync(_productId))
                .ReturnsAsync(product);

            IProductDomainService productDomainService = new ProductDomainService(productRepository.Object);

            // Act
            await productDomainService.RemovePictureFromProductAsync(_productId);

            //Assert
            product.ProductPicture.IsEmpty.Should().BeTrue();
            product.ProductPicture.Name.Should().Be(null);
            product.ProductPicture.Url.Should().Be(null);
            productRepository.Verify(pr => pr.GetByIdAsync(_productId), Times.Once);
            productRepository.Verify(pr => pr.Update(product), Times.Once);
        }
    }
}