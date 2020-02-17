using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Products;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Products;
using Shoppingendly.Services.Products.Core.Types;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Aggregates
{
    public class ProductTests
    {
        #region domain logic

        [Fact]
        public void CheckIfSetNameMethodReturnTrueWhenInputIsDifferentThenExistingValue()
        {
            // Arrange
            const string productName = "ExampleProductName";
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName", "ExampleProducer");

            // Act
            var testResult = product.SetName(productName);

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetNameMethodReturnFalseWhenInputIsTheSameThenExistingValue()
        {
            // Arrange
            const string productName = "ExampleProductName";
            var product = new Product(new ProductId(), new CreatorId(), productName, "ExampleProducer");

            // Act
            var testResult = product.SetName(productName);

            // Assert
            testResult.Should().BeFalse();
        }

        [Theory]
        [InlineData("Prod")]
        [InlineData("IProvideMaximalNumberOfLetters")]
        public void CheckIfSetProductNameMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
            string name)
        {
            // Arrange
            var productName = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetName(productName);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetProductNameMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string productName = "OtherProductName";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.SetName(productName);

            // Assert
            product.Name.Should().Be(productName);
            product.UpdatedDate.Should().NotBe(default);
            product.CreatedAt.Should().NotBe(default);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided(string name)
        {
            // Arrange
            var productName = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be empty.");
        }

        [Fact]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
        {
            // Arrange
            const string productName = "Hom";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be shorter than 4 characters.");
        }

        [Fact]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
        {
            // Arrange
            const string productName = "IProvideMaximalNumberOfLettersAndFewMore";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be longer than 30 characters.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodReturnTrueWhenInputIsDifferentThenExistingValue()
        {
            // Arrange
            const string productProducer = "ExampleProductProducer";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductProducer", "ExampleProducer");

            // Act
            var testResult = product.SetProducer(productProducer);

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetProductProducerMethodReturnFalseWhenInputIsTheSameThenExistingValue()
        {
            // Arrange
            const string productProducer = "ExampleProductProducer";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", productProducer);

            // Act
            var testResult = product.SetProducer(productProducer);

            // Assert
            testResult.Should().BeFalse();
        }

        [Theory]
        [InlineData("Prod")]
        [InlineData("IProvideMaximalNumberOfLetters")]
        public void CheckIfSetProductProducerMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
            string name)
        {
            // Arrange
            var productProducer = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetProducer(productProducer);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetProductProducerMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string productProducer = "OtherProducerName";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.SetProducer(productProducer);

            // Assert
            product.Producer.Should().Be(productProducer);
            product.UpdatedDate.Should().NotBe(default);
            product.CreatedAt.Should().NotBe(default);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided(
            string name)
        {
            // Arrange
            var productProducer = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetProducer(productProducer);

            // Assert
            func.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be empty.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
        {
            // Arrange
            const string producerName = "P";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetProducer(producerName);

            // Assert
            func.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be shorter than 2 characters.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
        {
            // Arrange
            const string producerName =
                "IProvideMaximalNumberOfLettersAndFewMoreBecauseProducerCanNotBeLongerThan50Characters";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetProducer(producerName);

            // Assert
            func.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be longer than 50 characters.");
        }

        [Fact]
        public void CheckIfGetAssignCategoryMethodReturnValidObjectWhenCorrectValueWasProvided()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            var expectedValue = new ProductCategory(product.Id, categoryId);
            product.ProductCategories.Add(expectedValue);

            // Act
            Func<Maybe<ProductCategory>> func = () => product.GetAssignedCategory(categoryId);
            var assignedCategory = func.Invoke();

            // Assert
            func.Should().NotThrow();
            assignedCategory.Should().NotBeNull();
            assignedCategory.Value.FirstKey.Should().Be(product.Id);
            assignedCategory.Value.SecondKey.Should().Be(categoryId);
        }
        
        [Fact]
        public void CheckIfAssignCategoryMethodDoNotThrowExceptionAndAddCorrectItemToList()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            var expectedValue = new ProductCategory(product.Id, categoryId);

            // Act
            Action action = () => product.AssignCategory(categoryId);
            
            // Assert
            action.Should().NotThrow();
            product.ProductCategories.Should().NotBeEmpty();
            product.UpdatedDate.Should().NotBe(default);
            var assignedProduct = product.ProductCategories.FirstOrDefault();
            assignedProduct?.FirstKey.Should().Be(expectedValue.FirstKey);
            assignedProduct?.SecondKey.Should().Be(expectedValue.SecondKey);
        }

        [Fact]
        public void CheckIfAssignCategoryMethodThrowExceptionWhenProductIsAlreadyAssignedToCategory()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            var newProductCategory = new ProductCategory(product.Id, categoryId);
            product.ProductCategories.Add(newProductCategory);
            
            // Act
            Action action = () => product.AssignCategory(categoryId);

            // Assert
            action.Should().Throw<ProductIsAlreadyAssignedToCategoryException>()
                .WithMessage($"Product already assigned to category with id: {categoryId.Id}.");
        }
        
        [Fact]
        public void CheckIfDeallocateCategoryMethodDoNotThrowExceptionAndRemoveCorrectItemToList()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            product.AssignCategory(categoryId);
            
            // Act
            Action action = () => product.DeallocateCategory(categoryId);
            
            // Assert
            action.Should().NotThrow();
            product.ProductCategories.Should().BeEmpty();
            product.UpdatedDate.Should().NotBe(default);
        }

        [Fact]
        public void CheckIfDeallocateCategoryMethodThrowExceptionWhenCategoryIsNotFound()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Action action = () => product.DeallocateCategory(categoryId);

            // Assert
            action.Should().Throw<ProductWithAssignedCategoryNotFoundException>()
                .WithMessage($"Product with assigned category with id: {categoryId.Id} not found.");
        }
        
        [Fact]
        public void CheckIfDeallocateFromAllCategoriesMethodDoNotThrowExceptionAndRemoveAllItemsFromList()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());
            var assignedCategoriesCount = product.ProductCategories.Count;
            
            // Act
            Action action = () => product.DeallocateAllCategories();
            
            // Assert
            action.Should().NotThrow();
            assignedCategoriesCount.Should().Be(2);
            product.ProductCategories.Should().BeEmpty();
            product.UpdatedDate.Should().NotBe(default);
        }

        [Fact]
        public void CheckIfDeallocateFromAllCategoriesMethodThrowExceptionWhenCategoryIsNotFound()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Action action = () => product.DeallocateAllCategories();

            // Assert
            action.Should().Throw<AnyProductWithAssignedCategoryNotFoundException>()
                .WithMessage("Unable to find any product with assigned category.");
        }

        [Fact]
        public void CheckIfGetAllAssignedCategoriesMethodReturnValuesAndDoNotThrown()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());
            
            // Act
            Func<Maybe<IEnumerable<ProductCategory>>> func = () => product.GetAllAssignedCategories();

            // Assert
            func.Should().NotThrow();
            var assignedCategories = func.Invoke();
            assignedCategories.Value.Should().HaveCount(2);
        }

        #endregion

        #region domain events

        [Fact]
        public void CheckIfCreateNewProductProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange

            // Act
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            var newCategoryCreatedDomainEvent =
                product.GetUncommitted().LastOrDefault() as NewProductCreatedDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            newCategoryCreatedDomainEvent.Should().BeOfType<NewProductCreatedDomainEvent>();
            newCategoryCreatedDomainEvent.Should().NotBeNull();
            newCategoryCreatedDomainEvent?.ProductId.Should().Be(product.Id);
            newCategoryCreatedDomainEvent?.CreatorId.Should().Be(product.CreatorId);
            newCategoryCreatedDomainEvent?.ProductName.Should().Be(product.Name);
            newCategoryCreatedDomainEvent?.ProductProducer.Should().Be(product.Producer);
        }

        [Fact]
        public void CheckIfSetProductNameMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.SetName("NewProductName");
            var productNameChangedDomainEvent =
                product.GetUncommitted().LastOrDefault() as ProductNameChangedDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productNameChangedDomainEvent.Should().BeOfType<ProductNameChangedDomainEvent>();
            productNameChangedDomainEvent.Should().NotBeNull();
            productNameChangedDomainEvent?.ProductId.Should().Be(product.Id);
            productNameChangedDomainEvent?.ProductName.Should().Be(product.Name);
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.SetProducer("Other producer");
            var productProducerChanged =
                product.GetUncommitted().LastOrDefault() as ProductProducerChangedDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productProducerChanged.Should().BeOfType<ProductProducerChangedDomainEvent>();
            productProducerChanged.Should().NotBeNull();
            productProducerChanged?.ProductId.Should().Be(product.Id);
            productProducerChanged?.ProductProducer.Should().Be(product.Producer);
        }
        
        [Fact]
        public void CheckIfAssignCategoryMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.AssignCategory(categoryId);
            var productAssignedToCategory =
                product.GetUncommitted().LastOrDefault() as ProductAssignedToCategoryDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productAssignedToCategory.Should().BeOfType<ProductAssignedToCategoryDomainEvent>();
            productAssignedToCategory.Should().NotBeNull();
            productAssignedToCategory?.ProductId.Should().Be(product.Id);
            productAssignedToCategory?.CategoryId.Should().Be(categoryId);
        }
        
        [Fact]
        public void CheckIfDeallocateCategoryMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            product.AssignCategory(categoryId);
            
            // Act
            product.DeallocateCategory(categoryId);
            var productDeallocatedFromCategory =
                product.GetUncommitted().LastOrDefault() as ProductDeallocatedFromCategoryDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productDeallocatedFromCategory.Should().BeOfType<ProductDeallocatedFromCategoryDomainEvent>();
            productDeallocatedFromCategory.Should().NotBeNull();
            productDeallocatedFromCategory?.ProductId.Should().Be(product.Id);
            productDeallocatedFromCategory?.CategoryId.Should().Be(categoryId);
        }
        
        [Fact]
        public void CheckIfDeallocateAllCategoriesMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());
            
            // Act
            product.DeallocateAllCategories();
            var productDeallocatedFromAllCategories =
                product.GetUncommitted().LastOrDefault() as ProductDeallocatedFromAllCategoriesDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productDeallocatedFromAllCategories.Should().BeOfType<ProductDeallocatedFromAllCategoriesDomainEvent>();
            productDeallocatedFromAllCategories.Should().NotBeNull();
            productDeallocatedFromAllCategories?.ProductId.Should().Be(product.Id);
            productDeallocatedFromAllCategories?.CategoriesIds.Should().HaveCount(2);
        }

        [Fact]
        public void CheckIfClearDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.ClearDomainEvents();

            // Assert
            product.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfGetUncommittedDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            var domainEvents = product.GetUncommitted().ToList();

            // Assert
            domainEvents.Should().NotBeNull();
            domainEvents.LastOrDefault().Should().BeOfType<NewProductCreatedDomainEvent>();
        }

        #endregion
    }
}