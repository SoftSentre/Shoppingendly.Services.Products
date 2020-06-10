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
using FluentAssertions;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Entities
{
    public class CategoryTests
    {
        [Theory]
        [InlineData("Home")]
        [InlineData("IProvideMaximalNumberOfLetters")]
        public void CheckIfSetCategoryNameMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
            string name)
        {
            // Arrange
            var categoryName = name;
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            Func<bool> func = () => category.SetCategoryName(categoryName);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfSetCategoryNameMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided(
            string categoryName)
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            Func<bool> func = () => category.SetCategoryName(categoryName);

            // Assert
            func.Should().Throw<CategoryNameCanNotBeEmptyException>()
                .WithMessage("Category name can not be empty.");
        }

        [Theory]
        [MemberData(nameof(CategoryDataGenerator.CorrectCategoryDescriptions),
            MemberType = typeof(CategoryDataGenerator))]
        public void
            CheckIfSetCategoryDescriptionMethodReturnTrueWhenCorrectDescriptionHasBeenProvidedAndDoNotThrowAnyException(
                string description)
        {
            // Arrange
            var categoryDescription = description;
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");

            // Act
            Func<bool> func = () => category.SetCategoryDescription(categoryDescription);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        private class CategoryDataGenerator
        {
            public static IEnumerable<object[]> CorrectCategoryDescriptions =>
                new List<object[]>
                {
                    new object[] {"Description is correct"},
                    new object[] {new string('*', 3999)}
                };
        }

        [Fact]
        public void CheckIfClearDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            category.ClearDomainEvents();

            // Assert
            category.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfCreateNewCategoryByConstructorWithDescriptionProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange

            // Act
            var category = new Category(new CategoryId(), "ExampleCategory", "Description is correct.");
            var newCategoryCreatedDomainEvent =
                category.GetUncommitted().LastOrDefault() as NewCategoryCreatedDomainEvent ??
                It.IsAny<NewCategoryCreatedDomainEvent>();

            // Assert
            category.DomainEvents.Should().NotBeEmpty();
            newCategoryCreatedDomainEvent.Should().BeOfType<NewCategoryCreatedDomainEvent>();
            newCategoryCreatedDomainEvent.Should().NotBeNull();
            newCategoryCreatedDomainEvent.CategoryId.Should().Be(category.CategoryId);
            newCategoryCreatedDomainEvent.CategoryName.Should().Be(category.CategoryName);
            newCategoryCreatedDomainEvent.CategoryDescription.Should().Be(category.CategoryDescription);
        }

        [Fact]
        public void
            CheckIfCreateNewCategoryByConstructorWithoutDescriptionProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange

            // Act
            var category = new Category(new CategoryId(), "ExampleCategory");
            var newCategoryCreatedDomainEvent =
                category.GetUncommitted().LastOrDefault() as NewCategoryCreatedDomainEvent ??
                It.IsAny<NewCategoryCreatedDomainEvent>();

            // Assert
            category.DomainEvents.Should().NotBeEmpty();
            newCategoryCreatedDomainEvent.Should().BeOfType<NewCategoryCreatedDomainEvent>();
            newCategoryCreatedDomainEvent.Should().NotBeNull();
            newCategoryCreatedDomainEvent.CategoryId.Should().Be(category.CategoryId);
            newCategoryCreatedDomainEvent.CategoryName.Should().Be(category.CategoryName);
            newCategoryCreatedDomainEvent.CategoryDescription.Should().Be(category.CategoryDescription);
        }

        [Fact]
        public void CheckIfGetUncommittedDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory", "Description is correct.");

            // Act
            var domainEvents = category.GetUncommitted().ToList();

            // Assert
            domainEvents.Should().NotBeNull();
            domainEvents.LastOrDefault().Should().BeOfType<NewCategoryCreatedDomainEvent>();
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory", "Description is correct.");

            // Act
            category.SetCategoryDescription("Other correct description");
            var categoryDescriptionChanged =
                category.GetUncommitted().LastOrDefault() as CategoryDescriptionChangedDomainEvent ??
                It.IsAny<CategoryDescriptionChangedDomainEvent>();

            // Assert
            category.DomainEvents.Should().NotBeEmpty();
            categoryDescriptionChanged.Should().BeOfType<CategoryDescriptionChangedDomainEvent>();
            categoryDescriptionChanged.Should().NotBeNull();
            categoryDescriptionChanged.CategoryId.Should().Be(category.CategoryId);
            categoryDescriptionChanged.CategoryDescription.Should().Be(category.CategoryDescription);
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodReturnFalseWhenParameterIsTheSameAsExistingValue()
        {
            // Arrange
            const string description = "Description is correct.";
            var category = new Category(new CategoryId(), "ExampleCategory", description);

            // Act
            var testResult = category.SetCategoryDescription(description);

            // Assert
            testResult.Should().BeFalse();
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodReturnTrueWhenParameterIsDifferentAsExistingValue()
        {
            // Arrange
            const string description = "Description is correct.";
            var category = new Category(new CategoryId(), "ExampleCategory", "Other description is correct.");

            // Act
            var testResult = category.SetCategoryDescription(description);

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodSetValuesWhenCorrectDescriptionHasBeenProvided()
        {
            // Arrange
            const string categoryDescription = "Description is correct.";
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");

            // Act
            category.SetCategoryDescription(categoryDescription);

            // Assert
            category.CategoryDescription.Should().Be(categoryDescription);
            category.UpdatedDate.Should().NotBe(default);
            category.CreatedAt.Should().NotBe(default);
        }

        [Fact]
        public void
            CheckIfSetCategoryDescriptionMethodThrowProperExceptionAndMessageWhenTooLongDescriptionHasBeenProvided()
        {
            // Arrange
            var description = new string('*', 4001);
            var category = new Category(new CategoryId(), "ExampleCategory", "Description is correct.");

            // Act
            Func<bool> func = () => category.SetCategoryDescription(description);

            // Assert
            func.Should().Throw<CategoryDescriptionIsTooLongException>()
                .WithMessage("Category description can not be longer than 4000 characters.");
        }

        [Fact]
        public void
            CheckIfSetCategoryDescriptionMethodThrowProperExceptionAndMessageWhenTooShortDescriptionHasBeenProvided()
        {
            // Arrange
            const string description = "Description is too";
            var category = new Category(new CategoryId(), "ExampleCategory", "Description is correct.");

            // Act
            Func<bool> func = () => category.SetCategoryDescription(description);

            // Assert
            func.Should().Throw<CategoryDescriptionIsTooShortException>()
                .WithMessage("Category description can not be shorter than 20 characters.");
        }

        [Fact]
        public void CheckIfSetCategoryNameMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            category.SetCategoryName("NewCategoryName");
            var categoryNameChangedDomainEvent =
                category.GetUncommitted().LastOrDefault() as CategoryNameChangedDomainEvent ??
                It.IsAny<CategoryNameChangedDomainEvent>();

            // Assert
            category.DomainEvents.Should().NotBeEmpty();
            categoryNameChangedDomainEvent.Should().BeOfType<CategoryNameChangedDomainEvent>();
            categoryNameChangedDomainEvent.Should().NotBeNull();
            categoryNameChangedDomainEvent.CategoryId.Should().Be(category.CategoryId);
            categoryNameChangedDomainEvent.CategoryName.Should().Be(category.CategoryName);
        }

        [Fact]
        public void CheckIfSetCategoryNameMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string categoryName = "OtherCategory";
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            category.SetCategoryName(categoryName);

            // Assert
            category.CategoryName.Should().Be(categoryName);
            category.UpdatedDate.Should().NotBe(default);
            category.CreatedAt.Should().NotBe(default);
        }

        [Fact]
        public void CheckIfSetCategoryNameMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
        {
            // Arrange
            const string categoryName = "IProvideMaximalNumberOfLettersAndFewMore";
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            Func<bool> func = () => category.SetCategoryName(categoryName);

            // Assert
            func.Should().Throw<CategoryNameIsTooLongException>()
                .WithMessage("Category name can not be longer than 30 characters.");
        }

        [Fact]
        public void CheckIfSetCategoryNameMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
        {
            // Arrange
            const string categoryName = "Hom";
            var category = new Category(new CategoryId(), "ExampleCategory");

            // Act
            Func<bool> func = () => category.SetCategoryName(categoryName);

            // Assert
            func.Should().Throw<CategoryNameIsTooShortException>()
                .WithMessage("Category name can not be shorter than 4 characters.");
        }

        [Fact]
        public void CheckIfSetNameMethodReturnFalseWhenParameterIsTheSameAsExistingValue()
        {
            // Arrange
            const string categoryName = "ExampleCategory";
            var category = new Category(new CategoryId(), categoryName);

            // Act
            var testResult = category.SetCategoryName(categoryName);

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
            var testResult = category.SetCategoryName(categoryName);

            // Assert
            testResult.Should().BeTrue();
        }
    }
}