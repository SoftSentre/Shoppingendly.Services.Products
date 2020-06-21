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
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Factories
{
    public class CategoryFactoryTests : IAsyncLifetime
    {
        private Mock<ICategoryBusinessRulesChecker> _categoryBusinessRulesCheckerMock;
        private Mock<IDomainEventEmitter> _domainEventEmitterMock;
        private CategoryFactory _categoryFactory;

        private CategoryId _categoryId;
        private string _categoryName;
        private string _categoryDescription;
        private Picture _categoryIcon;

        public async Task InitializeAsync()
        {
            _categoryBusinessRulesCheckerMock = new Mock<ICategoryBusinessRulesChecker>();
            _domainEventEmitterMock = new Mock<IDomainEventEmitter>();
            _categoryId = new CategoryId(new Guid("6B9AFDB4-F2E6-49F4-BD4D-A1AF17379FE4"));
            _categoryName = "exampleCategoryName";
            _categoryDescription = "exampleCategoryDescription";
            _categoryIcon = Picture.Create("exampleCategoryIconName", "exampleCategoryUrl");

            await Task.CompletedTask;
        }

        [Fact]
        public void SuccessToCreateCategoryWhenParametersAreCorrect()
        {
            // Arrange
            _categoryFactory =
                new CategoryFactory(_categoryBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Act
            var category = _categoryFactory.Create(_categoryId, _categoryName);

            // Assert
            _categoryBusinessRulesCheckerMock.Verify(cbc => cbc.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(cbc => cbc.CategoryNameCanNotBeEmptyRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryNameCanNotBeShorterThanRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryNameCanNotBeLongerThanRuleIsBroken(_categoryName),
                Times.Once());

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Category>(),
                    It.Is<NewCategoryCreatedDomainEvent>(de =>
                        de.CategoryId.Equals(category.CategoryId) && de.CategoryName == category.CategoryName &&
                        de.CategoryIcon == category.CategoryIcon &&
                        de.CategoryDescription == category.CategoryDescription)), Times.Once);
        }

        [Fact]
        public void SuccessToCreateCategoryWithDescriptionWhenParametersAreCorrect()
        {
            // Arrange
            _categoryFactory =
                new CategoryFactory(_categoryBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Act
            var category = _categoryFactory.Create(_categoryId, _categoryName, _categoryDescription);

            // Assert
            _categoryBusinessRulesCheckerMock.Verify(cbc => cbc.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(cbc => cbc.CategoryNameCanNotBeEmptyRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryNameCanNotBeShorterThanRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryNameCanNotBeLongerThanRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryDescriptionCanNotBeEmptyRuleIsBroken(_categoryDescription),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryDescriptionCanNotBeShorterThanRuleIsBroken(_categoryDescription),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryDescriptionCanNotBeLongerThanRuleIsBroken(_categoryDescription),
                Times.Once());

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Category>(),
                    It.Is<NewCategoryCreatedDomainEvent>(de =>
                        de.CategoryId.Equals(category.CategoryId) && de.CategoryName == category.CategoryName &&
                        de.CategoryIcon == category.CategoryIcon &&
                        de.CategoryDescription == category.CategoryDescription)), Times.Once);
        }

        [Fact]
        public void SuccessToCreateCategoryWithIconWhenParametersAreCorrect()
        {
            // Arrange
            _categoryFactory =
                new CategoryFactory(_categoryBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Act
            var category = _categoryFactory.Create(_categoryId, _categoryName, _categoryIcon);

            // Assert
            _categoryBusinessRulesCheckerMock.Verify(cbc => cbc.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(cbc => cbc.CategoryNameCanNotBeEmptyRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryNameCanNotBeShorterThanRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryNameCanNotBeLongerThanRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(_categoryIcon),
                Times.Once());

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Category>(),
                    It.Is<NewCategoryCreatedDomainEvent>(de =>
                        de.CategoryId.Equals(category.CategoryId) && de.CategoryName == category.CategoryName &&
                        de.CategoryIcon == category.CategoryIcon &&
                        de.CategoryDescription == category.CategoryDescription)), Times.Once);
        }

        [Fact]
        public void SuccessToCreateCategoryWithDescriptionAndIconWhenParametersAreCorrect()
        {
            // Arrange
            _categoryFactory =
                new CategoryFactory(_categoryBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Act
            var category = _categoryFactory.Create(_categoryId, _categoryName, _categoryDescription, _categoryIcon);

            // Assert
            _categoryBusinessRulesCheckerMock.Verify(cbc => cbc.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(cbc => cbc.CategoryNameCanNotBeEmptyRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryNameCanNotBeShorterThanRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryNameCanNotBeLongerThanRuleIsBroken(_categoryName),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryDescriptionCanNotBeEmptyRuleIsBroken(_categoryDescription),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryDescriptionCanNotBeShorterThanRuleIsBroken(_categoryDescription),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryDescriptionCanNotBeLongerThanRuleIsBroken(_categoryDescription),
                Times.Once());
            _categoryBusinessRulesCheckerMock.Verify(
                cbc => cbc.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(_categoryIcon),
                Times.Once());

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Category>(),
                    It.Is<NewCategoryCreatedDomainEvent>(de =>
                        de.CategoryId.Equals(category.CategoryId) && de.CategoryName == category.CategoryName &&
                        de.CategoryIcon == category.CategoryIcon &&
                        de.CategoryDescription == category.CategoryDescription)), Times.Once);
        }

        [Fact]
        private void FailToCreateCategoryWhenCategoryIdIsEmpty()
        {
            FailToCreateWhenCategoryIdIsEmpty(_categoryId, _categoryName);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionWhenCategoryIdIsEmpty()
        {
            FailToCreateWhenCategoryIdIsEmpty(_categoryId, _categoryName, _categoryDescription);
        }

        [Fact]
        private void FailToCreateCategoryWithIconWhenCategoryIdIsEmpty()
        {
            FailToCreateWhenCategoryIdIsEmpty(_categoryId, _categoryName, categoryIcon: _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionAndIconWhenCategoryIdIsEmpty()
        {
            FailToCreateWhenCategoryIdIsEmpty(_categoryId, _categoryName, _categoryDescription, _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWhenCategoryNameIsEmpty()
        {
            FailToCreateWhenCategoryNameIsEmpty(_categoryId, _categoryName);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionWhenCategoryNameIsEmpty()
        {
            FailToCreateWhenCategoryNameIsEmpty(_categoryId, _categoryName, _categoryDescription);
        }

        [Fact]
        private void FailToCreateCategoryWithIconWhenCategoryNameIsEmpty()
        {
            FailToCreateWhenCategoryNameIsEmpty(_categoryId, _categoryName, categoryIcon: _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionAndIconWhenCategoryNameIsEmpty()
        {
            FailToCreateWhenCategoryNameIsEmpty(_categoryId, _categoryName, _categoryDescription, _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWhenCategoryNameIsTooShort()
        {
            FailToCreateWhenCategoryNameIsTooShort(_categoryId, _categoryName);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionWhenCategoryNameIsTooShort()
        {
            FailToCreateWhenCategoryNameIsTooShort(_categoryId, _categoryName, _categoryDescription);
        }

        [Fact]
        private void FailToCreateCategoryWithIconWhenCategoryNameIsTooShort()
        {
            FailToCreateWhenCategoryNameIsTooShort(_categoryId, _categoryName, categoryIcon: _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionAndIconWhenCategoryNameIsTooShort()
        {
            FailToCreateWhenCategoryNameIsTooShort(_categoryId, _categoryName, _categoryDescription, _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWhenCategoryNameIsTooLong()
        {
            FailToCreateWhenCategoryNameIsTooLong(_categoryId, _categoryName);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionWhenCategoryNameIsTooLong()
        {
            FailToCreateWhenCategoryNameIsTooLong(_categoryId, _categoryName, _categoryDescription);
        }

        [Fact]
        private void FailToCreateCategoryWithIconWhenCategoryNameIsTooLong()
        {
            FailToCreateWhenCategoryNameIsTooLong(_categoryId, _categoryName, categoryIcon: _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionAndIconWhenCategoryNameIsTooLong()
        {
            FailToCreateWhenCategoryNameIsTooLong(_categoryId, _categoryName, _categoryDescription, _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWhenCategoryDescriptionIsEmpty()
        {
            FailToCreateWhenCategoryDescriptionIsEmpty(_categoryId, _categoryName);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionWhenCategoryDescriptionIsEmpty()
        {
            FailToCreateWhenCategoryDescriptionIsEmpty(_categoryId, _categoryName, _categoryDescription);
        }

        [Fact]
        private void FailToCreateCategoryWithIconWhenCategoryDescriptionIsEmpty()
        {
            FailToCreateWhenCategoryDescriptionIsEmpty(_categoryId, _categoryName, categoryIcon: _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionAndIconWhenCategoryDescriptionIsEmpty()
        {
            FailToCreateWhenCategoryDescriptionIsEmpty(_categoryId, _categoryName, _categoryDescription, _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWhenCategoryDescriptionIsTooShort()
        {
            FailToCreateWhenCategoryDescriptionIsTooShort(_categoryId, _categoryName);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionWhenCategoryDescriptionIsTooShort()
        {
            FailToCreateWhenCategoryDescriptionIsTooShort(_categoryId, _categoryName, _categoryDescription);
        }

        [Fact]
        private void FailToCreateCategoryWithIconWhenCategoryDescriptionIsTooShort()
        {
            FailToCreateWhenCategoryDescriptionIsTooShort(_categoryId, _categoryName, categoryIcon: _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionAndIconWhenCategoryDescriptionIsTooShort()
        {
            FailToCreateWhenCategoryDescriptionIsTooShort(_categoryId, _categoryName, _categoryDescription,
                _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWhenCategoryDescriptionIsTooLong()
        {
            FailToCreateWhenCategoryDescriptionIsTooLong(_categoryId, _categoryName);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionWhenCategoryDescriptionIsTooLong()
        {
            FailToCreateWhenCategoryDescriptionIsTooLong(_categoryId, _categoryName, _categoryDescription);
        }

        [Fact]
        private void FailToCreateCategoryWithIconWhenCategoryDescriptionIsTooLong()
        {
            FailToCreateWhenCategoryDescriptionIsTooLong(_categoryId, _categoryName, categoryIcon: _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionAndIconWhenCategoryDescriptionIsTooLong()
        {
            FailToCreateWhenCategoryDescriptionIsTooLong(_categoryId, _categoryName, _categoryDescription,
                _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWhenCategoryIconIsNullOrEmpty()
        {
            FailToCreateWhenCategoryIconIsNullOrEmpty(_categoryId, _categoryName);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionWhenCategoryIconIsNullOrEmpty()
        {
            FailToCreateWhenCategoryIconIsNullOrEmpty(_categoryId, _categoryName, _categoryDescription);
        }

        [Fact]
        private void FailToCreateCategoryWithIconWhenCategoryIconIsNullOrEmpty()
        {
            FailToCreateWhenCategoryIconIsNullOrEmpty(_categoryId, _categoryName, categoryIcon: _categoryIcon);
        }

        [Fact]
        private void FailToCreateCategoryWithDescriptionAndIconWhenCategoryIconIsNullOrEmpty()
        {
            FailToCreateWhenCategoryIconIsNullOrEmpty(_categoryId, _categoryName, _categoryDescription,
                _categoryIcon);
        }

        private void FailToCreateWhenCategoryIdIsEmpty(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            FailToCreateCategoryWithBasicParametersWhenBusinessRuleHasBeenBroken(
                checker => checker.CategoryIdCanNotBeEmptyRuleIsBroken(It.IsAny<CategoryId>()),
                new InvalidCategoryIdException(categoryId), categoryId, categoryName, categoryDescription,
                categoryIcon);
        }

        private void FailToCreateWhenCategoryNameIsEmpty(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            FailToCreateCategoryWithBasicParametersWhenBusinessRuleHasBeenBroken(
                checker => checker.CategoryNameCanNotBeEmptyRuleIsBroken(It.IsAny<string>()),
                new CategoryNameCanNotBeEmptyException(), categoryId, categoryName, categoryDescription, categoryIcon);
        }

        private void FailToCreateWhenCategoryNameIsTooShort(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            FailToCreateCategoryWithBasicParametersWhenBusinessRuleHasBeenBroken(
                checker => checker.CategoryNameCanNotBeShorterThanRuleIsBroken(It.IsAny<string>()),
                new CategoryNameIsTooShortException(GlobalValidationVariables.CategoryNameMinLength), categoryId,
                categoryName, categoryDescription, categoryIcon);
        }

        private void FailToCreateWhenCategoryNameIsTooLong(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            FailToCreateCategoryWithBasicParametersWhenBusinessRuleHasBeenBroken(
                checker => checker.CategoryNameCanNotBeLongerThanRuleIsBroken(It.IsAny<string>()),
                new CategoryNameIsTooLongException(GlobalValidationVariables.CategoryNameMaxLength), categoryId,
                categoryName, categoryDescription, categoryIcon);
        }

        private void FailToCreateWhenCategoryDescriptionIsEmpty(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            FailToCreateCategoryWithDescriptionWhenBusinessRuleHasBeenBroken(
                checker => checker.CategoryDescriptionCanNotBeEmptyRuleIsBroken(It.IsAny<string>()),
                new CategoryDescriptionCanNotBeEmptyException(), categoryId, categoryName, categoryDescription,
                categoryIcon);
        }

        private void FailToCreateWhenCategoryDescriptionIsTooShort(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            FailToCreateCategoryWithDescriptionWhenBusinessRuleHasBeenBroken(
                checker => checker.CategoryDescriptionCanNotBeShorterThanRuleIsBroken(It.IsAny<string>()),
                new CategoryDescriptionIsTooShortException(GlobalValidationVariables.CategoryDescriptionMinLength),
                categoryId, categoryName, categoryDescription, categoryIcon);
        }

        private void FailToCreateWhenCategoryDescriptionIsTooLong(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            FailToCreateCategoryWithDescriptionWhenBusinessRuleHasBeenBroken(
                checker => checker.CategoryDescriptionCanNotBeLongerThanRuleIsBroken(It.IsAny<string>()),
                new CategoryDescriptionIsTooLongException(GlobalValidationVariables.CategoryDescriptionMaxLength),
                categoryId, categoryName, categoryDescription, categoryIcon);
        }

        private void FailToCreateWhenCategoryIconIsNullOrEmpty(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            // Act
            _categoryBusinessRulesCheckerMock
                .Setup(cbr => cbr.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(categoryIcon))
                .Returns(true);
            _categoryFactory =
                new CategoryFactory(_categoryBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Action createCreator = () =>
                ChooseMethodToCreateCategory(categoryId, categoryName, categoryDescription, categoryIcon);

            // Assert
            if (categoryIcon != null && !categoryIcon.IsEmpty)
            {
                createCreator.Should().Throw<CategoryIconCanNotBeNullOrEmptyException>()
                    .Where(e => e.Code == ErrorCodes.CategoryIconCanNotBeNullOrEmpty)
                    .WithMessage("Category Icon can not be null or empty.");
                _categoryBusinessRulesCheckerMock.Verify(
                    cbr => cbr.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(categoryIcon), Times.Once);
            }
            else
                _categoryBusinessRulesCheckerMock.Verify(
                    cbr => cbr.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(categoryIcon), Times.Never);

            VerifyIfDomainEventHasBeenEmitted();
        }

        private void FailToCreateCategoryWithDescriptionWhenBusinessRuleHasBeenBroken<T>(
            Expression<Func<ICategoryBusinessRulesChecker, bool>> brokenRule, T exception, CategoryId categoryId,
            string categoryName, string categoryDescription = null, Picture categoryIcon = null)
            where T : DomainException
        {
            // Act
            _categoryBusinessRulesCheckerMock.Setup(brokenRule).Returns(true);
            _categoryFactory =
                new CategoryFactory(_categoryBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Action createCreator = () =>
                ChooseMethodToCreateCategory(categoryId, categoryName, categoryDescription, categoryIcon);

            // Assert
            if (categoryDescription.IsNotEmpty())
            {
                createCreator.Should().Throw<T>()
                    .Where(e => e.Code == exception.Code)
                    .WithMessage(exception.Message);

                _categoryBusinessRulesCheckerMock.Verify(brokenRule, Times.Once);
            }
            else
                _categoryBusinessRulesCheckerMock.Verify(brokenRule, Times.Never);

            VerifyIfDomainEventHasBeenEmitted();
        }

        private void FailToCreateCategoryWithBasicParametersWhenBusinessRuleHasBeenBroken<T>(
            Expression<Func<ICategoryBusinessRulesChecker, bool>> brokenRule, T exception, CategoryId categoryId,
            string categoryName, string categoryDescription = null, Picture categoryIcon = null)
            where T : DomainException
        {
            // Act
            _categoryBusinessRulesCheckerMock.Setup(brokenRule).Returns(true);
            _categoryFactory =
                new CategoryFactory(_categoryBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Action createCreator = () =>
                ChooseMethodToCreateCategory(categoryId, categoryName, categoryDescription, categoryIcon);

            // Assert
            createCreator.Should().Throw<T>()
                .Where(e => e.Code == exception.Code)
                .WithMessage(exception.Message);

            _categoryBusinessRulesCheckerMock.Verify(brokenRule, Times.Once);
            VerifyIfDomainEventHasBeenEmitted();
        }

        private void ChooseMethodToCreateCategory(CategoryId categoryId, string categoryName,
            string categoryDescription, Picture categoryIcon)
        {
            if (categoryDescription.IsEmpty() && categoryIcon == null)
                _categoryFactory.Create(categoryId, categoryName);
            else if (categoryDescription.IsEmpty() && categoryIcon != null)
                _categoryFactory.Create(categoryId, categoryName, categoryIcon);
            else if (categoryIcon == null && categoryDescription.IsNotEmpty())
                _categoryFactory.Create(categoryId, categoryName, categoryDescription);
            else
                _categoryFactory.Create(categoryId, categoryName, categoryDescription, categoryIcon);
        }

        private void VerifyIfDomainEventHasBeenEmitted()
        {
            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Category>(), It.IsAny<NewCategoryCreatedDomainEvent>()),
                Times.Never);
        }

        public async Task DisposeAsync()
        {
            _categoryBusinessRulesCheckerMock = null;
            _categoryId = null;
            _categoryName = null;
            _categoryDescription = null;
            _categoryIcon = null;

            await Task.CompletedTask;
        }
    }
}