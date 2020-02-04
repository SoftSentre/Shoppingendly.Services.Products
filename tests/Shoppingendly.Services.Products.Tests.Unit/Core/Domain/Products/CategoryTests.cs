using System;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Products.Entities;
using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Products
{
    public class CategoryTests
    {
        [Fact]
        public void CheckIfSetNameMethodReturnFalseWhenParameterIsTheSameAsExistingValue()
        {
            // Arrange
            const string categoryName = "ExampleCategory";
            var category = new Category(new CategoryId(), categoryName, string.Empty);

            // Act
            var testResult = category.SetName(categoryName);

            // Assert
            testResult.Should().BeFalse();
        }

        [Fact]
        public void CheckIfSetNameMethodReturnTrueWhenParameterIsDifferentAsExistingValue()
        {
            // Arrange
            const string categoryName = "OtherCategory";
            var category = new Category(new CategoryId(), "ExampleCategory", string.Empty);

            // Act
            var testResult = category.SetName(categoryName);

            // Assert
            testResult.Should().BeTrue();
        }

        [Theory]
        [InlineData("Home")]
        [InlineData("IProvideMaximalNumberOfLetters")]
        public void CheckIfSetNameMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(string name)
        {
            // Arrange
            var categoryName = name;
            var category = new Category(new CategoryId(), "ExampleCategory", string.Empty);

            // Act
            Func<bool> func = () => category.SetName(categoryName);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetNameMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided()
        {
            // Arrange
            var categoryName = string.Empty;
            var category = new Category(new CategoryId(), "ExampleCategory", string.Empty);

            // Act
            Func<bool> func = () => category.SetName(categoryName);

            // Assert
            func.Should().Throw<InvalidCategoryNameException>()
                .WithMessage("Category name can not be empty.");
        }

        [Fact]
        public void CheckIfSetNameMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
        {
            // Arrange
            const string categoryName = "Hom";
            var category = new Category(new CategoryId(), "ExampleCategory", string.Empty);

            // Act
            Func<bool> func = () => category.SetName(categoryName);

            // Assert
            func.Should().Throw<InvalidCategoryNameException>()
                .WithMessage("Category name can not be shorter than 4 characters.");
        }

        [Fact]
        public void CheckIfSetNameMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
        {
            // Arrange
            const string categoryName = "IProvideMaximalNumberOfLettersAndFewMore";
            var category = new Category(new CategoryId(), "ExampleCategory", string.Empty);

            // Act
            Func<bool> func = () => category.SetName(categoryName);

            // Assert
            func.Should().Throw<InvalidCategoryNameException>()
                .WithMessage("Category name can not be longer than 30 characters.");
        }
    }
}