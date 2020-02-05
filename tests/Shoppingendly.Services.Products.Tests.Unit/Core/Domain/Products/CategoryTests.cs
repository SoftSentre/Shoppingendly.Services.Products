using System;
using System.Collections.Generic;
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
            var category = new Category(new CategoryId(), categoryName);

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
            var category = new Category(new CategoryId(), "ExampleCategory");

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
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            Func<bool> func = () => category.SetName(categoryName);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }
        
        [Fact]
        public void CheckIfSetNameMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string categoryName = "OtherCategory";
            var category = new Category(new CategoryId(), "ExampleCategory");
            
            // Act
            category.SetName(categoryName);
            var isAssigned = category.Name == categoryName;
            var updatedDateAreChanged = category.UpdatedDate != default;
            
            // Assert
            isAssigned.Should().BeTrue();
            updatedDateAreChanged.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetNameMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided()
        {
            // Arrange
            var categoryName = string.Empty;
            var category = new Category(new CategoryId(), "ExampleCategory");

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
            var category = new Category(new CategoryId(), "ExampleCategory");

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
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            Func<bool> func = () => category.SetName(categoryName);

            // Assert
            func.Should().Throw<InvalidCategoryNameException>()
                .WithMessage("Category name can not be longer than 30 characters.");
        }

        [Fact]
        public void CheckIfSetDescriptionMethodReturnFalseWhenParameterIsTheSameAsExistingValue()
        {
            // Arrange
            const string description = "Description is correct.";
            var category = new Category(new CategoryId(), "ExampleCategory", description);

            // Act
            var testResult = category.SetDescription(description);

            // Assert
            testResult.Should().BeFalse();
        }

        [Fact]
        public void CheckIfSetDescriptionMethodReturnTrueWhenParameterIsDifferentAsExistingValue()
        {
            // Arrange
            const string description = "Description is correct.";
            var category = new Category(new CategoryId(), "ExampleCategory", "Other description is correct.");

            // Act
            var testResult = category.SetDescription(description);

            // Assert
            testResult.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(CategoryDataGenerator.CorrectCategoryDescriptions),
            MemberType = typeof(CategoryDataGenerator))]
        public void CheckIfSetDescriptionMethodReturnTrueWhenCorrectDescriptionHasBeenProvidedAndDoNotThrowAnyException(
            string description)
        {
            // Arrange
            var categoryDescription = description;
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");

            // Act
            Func<bool> func = () => category.SetDescription(categoryDescription);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetDescriptionMethodSetValuesWhenCorrectDescriptionHasBeenProvided()
        {
            // Arrange
            const string categoryDescription = "Description is correct.";
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");
            
            // Act
            category.SetDescription(categoryDescription);
            var isAssigned = category.Description == categoryDescription;
            var updatedDateAreChanged = category.UpdatedDate != default;
            
            // Assert
            isAssigned.Should().BeTrue();
            updatedDateAreChanged.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetDescriptionMethodThrowProperExceptionAndMessageWhenTooShortDescriptionHasBeenProvided()
        {
            // Arrange
            const string description = "Description is too";
            var category = new Category(new CategoryId(), "ExampleCategory", "Description is correct.");

            // Act
            Func<bool> func = () => category.SetDescription(description);

            // Assert
            func.Should().Throw<InvalidCategoryDescriptionException>()
                .WithMessage("Category description can not be shorter than 20 characters.");
        }
        
        [Fact]
        public void CheckIfSetDescriptionMethodThrowProperExceptionAndMessageWhenTooLongDescriptionHasBeenProvided()
        {
            // Arrange
            var description = new string('*', 4001);
            var category = new Category(new CategoryId(), "ExampleCategory", "Description is correct.");

            // Act
            Func<bool> func = () => category.SetDescription(description);

            // Assert
            func.Should().Throw<InvalidCategoryDescriptionException>()
                .WithMessage("Category description can not be longer than 4000 characters.");
        }
    }

    public class CategoryDataGenerator
    {
        public static IEnumerable<object[]> CorrectCategoryDescriptions =>
            new List<object[]>
            {
                new object[] {"Description is correct"},
                new object[] {new string('*', 3999)}
            };
    }
}