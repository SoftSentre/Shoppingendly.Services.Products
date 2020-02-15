using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Services;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services.Products;
using Shoppingendly.Services.Products.Core.Types;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Services
{
    public class ProductDomainServiceTests
    {
        [Fact]
        public void CheckIfGetAssignedCategoryMethodReturnNoValueWhenProductHasNotAssignAnyCategory()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<Maybe<ProductCategory>> func = () => productDomainService.GetAssignedCategory(product, categoryId);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.HasValue.Should().BeFalse();
        }

        [Fact]
        public void CheckIfGetAssignedCategoryMethodReturnValueWhenProductHasAssignSomeCategory()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            product.ProductCategories.Add(ProductCategory.Create(product.Id, categoryId));
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<Maybe<ProductCategory>> func = () => productDomainService.GetAssignedCategory(product, categoryId);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.HasValue.Should().BeTrue();
            testResult.Value.FirstKey.Should().Be(product.Id);
            testResult.Value.SecondKey.Should().Be(categoryId);
        }
        
        [Fact]
        public void CheckIfGetAssignedCategoryMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Action action = () => productDomainService.GetAssignedCategory(null, new CategoryId());

            //Assert
            action.Should().Throw<EmptyProductProvidedException>()
                .WithMessage("Unable to change product name, because provided value is empty.");
        }
        
        [Fact]
        public void CheckIfGetAssignedCategoriesMethodReturnNoValueWhenProductHasNotAssignAnyCategory()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<Maybe<IEnumerable<ProductCategory>>> func = () => productDomainService.GetAssignedCategories(product);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.HasValue.Should().BeTrue();
            testResult.Value.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfGetAssignedCategoriesMethodReturnValueWhenProductHasAssignSomeCategory()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            product.ProductCategories.Add(ProductCategory.Create(product.Id, new CategoryId()));
            product.ProductCategories.Add(ProductCategory.Create(product.Id, new CategoryId()));
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<Maybe<IEnumerable<ProductCategory>>> func = () => productDomainService.GetAssignedCategories(product);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.HasValue.Should().BeTrue();
            testResult.Value.Should().HaveCount(2);
        }
        
        [Fact]
        public void CheckIfGetAssignedCategoriesMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Action action = () => productDomainService.GetAssignedCategories(null);

            //Assert
            action.Should().Throw<EmptyProductProvidedException>()
                .WithMessage("Unable to change product name, because provided value is empty.");
        }

        [Fact]
        public void CheckIfAddNewProductMethodWithNoCategoriesCreateValidObjectAndDoNotThrown()
        {
            // Arrange
            var productId = new ProductId();
            var creatorId = new CreatorId();
            const string productName = "ExampleProductName";
            const string productProducer = "ExampleProductProducer";
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<Maybe<Product>> func = () =>
                productDomainService.AddNewProduct(productId, creatorId, productName, productProducer);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.Value.Id.Should().Be(productId);
            testResult.Value.CreatorId.Should().Be(creatorId);
            testResult.Value.Name.Should().Be(productName);
            testResult.Value.Producer.Should().Be(productProducer);
            testResult.Value.CreatedAt.Should().NotBe(default);
        }
        
        [Fact]
        public void CheckIfAddNewProductMethodWithCategoriesCreateValidObjectAndDoNotThrown()
        {
            // Arrange
            var productId = new ProductId();
            var creatorId = new CreatorId();
            var firstAssignedCategory = new CategoryId();
            var secondAssignedCategory = new CategoryId();
            const string productName = "ExampleProductName";
            const string productProducer = "ExampleProductProducer";
            IEnumerable<CategoryId> categories = new[] {firstAssignedCategory, secondAssignedCategory};
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<Maybe<Product>> func = () =>
                productDomainService.AddNewProduct(productId, creatorId, productName, productProducer, categories);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.Value.Id.Should().Be(productId);
            testResult.Value.CreatorId.Should().Be(creatorId);
            testResult.Value.Name.Should().Be(productName);
            testResult.Value.Producer.Should().Be(productProducer);
            testResult.Value.CreatedAt.Should().NotBe(default);
            var expectedFirstCategory =
                productDomainService.GetAssignedCategory(testResult.Value, firstAssignedCategory);
            expectedFirstCategory.Value.Should().NotBeNull();
            var expectedSecondCategory =
                productDomainService.GetAssignedCategory(testResult.Value, secondAssignedCategory);
            expectedFirstCategory.Value.Should().NotBeNull();
        }

        [Fact]
        public void CheckIfChangeProductNameMethodReturnTrueAndSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newProductName = "OtherExampleProductName";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<bool> func = () => productDomainService.ChangeProductName(product, newProductName);

            //Assert
            var testResult = func.Invoke();
            testResult.Should().BeTrue();
            product.Name.Should().Be("OtherExampleProductName");
        }

        [Fact]
        public void CheckIfChangeProductNameMethodDoNotThrownAnyException()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<bool> action = () => productDomainService.ChangeProductName(product, "OtherExampleProductName");

            //Assert
            action.Should().NotThrow();
        }
        
        [Fact]
        public void CheckIfChangeProductNameMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Action action = () => productDomainService.ChangeProductName(null, "ExampleProductName");

            //Assert
            action.Should().Throw<EmptyProductProvidedException>()
                .WithMessage("Unable to change product name, because provided value is empty.");
        }
        
        [Fact]
        public void CheckIfChangeProductProducerMethodReturnTrueAndSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newProductProducer = "OtherExampleProductProducer";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<bool> func = () => productDomainService.ChangeProductProducer(product, newProductProducer);

            //Assert
            var testResult = func.Invoke();
            testResult.Should().BeTrue();
            product.Producer.Should().Be("OtherExampleProductProducer");
        }

        [Fact]
        public void CheckIfChangeProductProducerMethodDoNotThrownAnyException()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Func<bool> action = () => productDomainService.ChangeProductProducer(product, "OtherExampleProductProducer");

            //Assert
            action.Should().NotThrow();
        }
        
        [Fact]
        public void CheckIfChangeProductProducerMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Action action = () => productDomainService.ChangeProductProducer(null, "ExampleProducerName");

            //Assert
            action.Should().Throw<EmptyProductProvidedException>()
                .WithMessage("Unable to change product name, because provided value is empty.");
        }

        [Fact]
        public void CheckIfAssignProductToCategoryMethodAddedOneElementToList()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            productDomainService.AssignProductToCategory(product, categoryId);

            //Assert
            product.ProductCategories.Should().NotBeEmpty();
            var expectedValue = product.ProductCategories.FirstOrDefault();
            expectedValue?.FirstKey.Should().Be(product.Id);
            expectedValue?.SecondKey.Should().Be(categoryId);
        }

        [Fact]
        public void CheckIfAssignProductToCategoryMethodDoNotThrown()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Action action = () => productDomainService.AssignProductToCategory(product, new CategoryId());

            //Assert
            action.Should().NotThrow();
        }
        
        [Fact]
        public void CheckIfAssignProductToCategoryMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Action action = () => productDomainService.AssignProductToCategory(null, new CategoryId());

            //Assert
            action.Should().Throw<EmptyProductProvidedException>()
                .WithMessage("Unable to change product name, because provided value is empty.");
        }
        
        [Fact]
        public void CheckIfDeallocatedProductToCategoryMethodAddedOneElementToList()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();
            product.AssignCategory(categoryId);

            // Act
            productDomainService.DeallocateProductFromCategory(product, categoryId);

            //Assert
            product.ProductCategories.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfDeallocatedProductToCategoryMethodDoNotThrown()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();
            product.AssignCategory(categoryId);

            // Act
            Action action = () => productDomainService.DeallocateProductFromCategory(product, categoryId);

            //Assert
            action.Should().NotThrow();
        }
        
        [Fact]
        public void CheckIfDeallocateProductFromCategoryMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Action action = () => productDomainService.DeallocateProductFromCategory(null, new CategoryId());

            //Assert
            action.Should().Throw<EmptyProductProvidedException>()
                .WithMessage("Unable to change product name, because provided value is empty.");
        }
        
        [Fact]
        public void CheckIfDeallocatedProductFromAllCategoriesMethodAddedOneElementToList()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());

            // Act
            productDomainService.DeallocateProductFromAllCategories(product);

            //Assert
            product.ProductCategories.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfDeallocatedProductFromAllCategoriesMethodDoNotThrown()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            IProductDomainService productDomainService = new ProductDomainService();
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());

            // Act
            Action action = () => productDomainService.DeallocateProductFromAllCategories(product);

            //Assert
            action.Should().NotThrow();
        }
        
        [Fact]
        public void CheckIfDeallocateProductFromAllCategoriesMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            IProductDomainService productDomainService = new ProductDomainService();

            // Act
            Action action = () => productDomainService.DeallocateProductFromAllCategories(null);

            //Assert
            action.Should().Throw<EmptyProductProvidedException>()
                .WithMessage("Unable to change product name, because provided value is empty.");
        }
    }
}