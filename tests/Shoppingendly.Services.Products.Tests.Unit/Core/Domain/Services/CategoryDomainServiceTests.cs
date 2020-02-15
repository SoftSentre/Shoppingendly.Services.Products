using System;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Services;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services;
using Shoppingendly.Services.Products.Core.Types;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Services
{
    public class CategoryDomainServiceTests
    {
        [Fact]
        public void CheckIfCreateNewCategoryMethodCreateValidObjectAndDoNotThrown()
        {
            // Arrange
            var categoryId = new CategoryId();
            const string categoryName = "ExampleCategoryName";
            ICategoryDomainService categoryDomainService = new CategoryDomainService();

            // Act
            Func<Maybe<Category>> func = () =>
                categoryDomainService.CreateNewCategory(categoryId, categoryName);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.Value.Id.Should().Be(categoryId);
            testResult.Value.Name.Should().Be(categoryName);
            testResult.Value.Description.Should().Be(null);
            testResult.Value.CreatedAt.Should().NotBe(default);
        }
        
        [Fact]
        public void CheckIfCreateNewCategoryMethodWithDescriptionCreateValidObjectAndDoNotThrown()
        {
            // Arrange
            var categoryId = new CategoryId();
            const string categoryName = "ExampleCategoryName";
            const string categoryDescription = "ExampleCategoryDescription";
            ICategoryDomainService categoryDomainService = new CategoryDomainService();

            // Act
            Func<Maybe<Category>> func = () =>
                categoryDomainService.CreateNewCategory(categoryId, categoryName, categoryDescription);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.Value.Id.Should().Be(categoryId);
            testResult.Value.Name.Should().Be(categoryName);
            testResult.Value.Description.Should().Be(categoryDescription);
            testResult.Value.CreatedAt.Should().NotBe(default);
        }

        [Fact]
        public void CheckIfChangeCategoryNameMethodReturnTrueAndSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newCategoryName = "OtherExampleCategoryName";
            var category = new Category(new CategoryId(), "ExampleProductName");
            ICategoryDomainService categoryDomainService = new CategoryDomainService();

            // Act
            Func<bool> func = () => categoryDomainService.SetCategoryName(category, newCategoryName);

            //Assert
            var testResult = func.Invoke();
            testResult.Should().BeTrue();
            category.Name.Should().Be("OtherExampleCategoryName");
        }

        [Fact]
        public void CheckIfChangeCategoryNameMethodDoNotThrownAnyException()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategoryName");
            ICategoryDomainService categoryDomainService = new CategoryDomainService();

            // Act
            Func<bool> action = () => categoryDomainService.SetCategoryName(category, "OtherExampleCategoryName");

            //Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void CheckIfSetCategoryNameMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            ICategoryDomainService categoryDomainService = new CategoryDomainService();

            // Act
            Action action = () => categoryDomainService.SetCategoryName(null, "ExampleCategoryName");

            //Assert
            action.Should().Throw<EmptyCategoryProvidedException>()
                .WithMessage("Unable to mutate category state, because provided value is empty.");
        }

        [Fact]
        public void CheckIfChangeCategoryDescriptionMethodReturnTrueAndSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newCategoryDescription = "OtherExampleCategoryDescription";
            var category = new Category(new CategoryId(), "ExampleCategoryDescription");
            ICategoryDomainService categoryDomainService = new CategoryDomainService();

            // Act
            Func<bool> func = () => categoryDomainService.SetCategoryDescription(category, newCategoryDescription);

            //Assert
            var testResult = func.Invoke();
            testResult.Should().BeTrue();
            category.Description.Should().Be("OtherExampleCategoryDescription");
        }

        [Fact]
        public void CheckIfChangeCategoryDescriptionMethodDoNotThrownAnyException()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategoryDescription");
            ICategoryDomainService categoryDomainService = new CategoryDomainService();

            // Act
            Func<bool> action = () =>
                categoryDomainService.SetCategoryDescription(category, "OtherExampleCategoryDescription");

            //Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            ICategoryDomainService categoryDomainService = new CategoryDomainService();

            // Act
            Action action = () => categoryDomainService.SetCategoryDescription(null, "ExampleCategoryDescription");

            //Assert
            action.Should().Throw<EmptyCategoryProvidedException>()
                .WithMessage("Unable to mutate category state, because provided value is empty.");
        }
    }
}