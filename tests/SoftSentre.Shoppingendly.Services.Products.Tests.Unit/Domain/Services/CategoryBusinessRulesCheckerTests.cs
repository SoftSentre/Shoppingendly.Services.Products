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
using System.Threading.Tasks;
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Services
{
    public class CategoryBusinessRulesCheckerTests : IAsyncLifetime
    {
        private ICategoryBusinessRulesChecker _categoryBusinessRulesChecker;

        private CategoryId _categoryId;
        private string _categoryName;
        private string _categoryDescription;
        private Picture _categoryIcon;

        public static IEnumerable<object[]> Icons =>
            new[]
            {
                new object[] {null},
                new object[] {Picture.Empty}
            };

        public async Task InitializeAsync()
        {
            _categoryBusinessRulesChecker = new CategoryBusinessRulesChecker();
            _categoryId = new CategoryId(new Guid("205DC0DB-E3A8-478D-9644-868B5F7ED6AA"));
            _categoryName = "exampleCategoryName";
            _categoryDescription = "exampleCategoryDescription";
            _categoryIcon = Picture.Create("exampleCategoryIconName", "exampleCategoryIconUrl");

            await Task.CompletedTask;
        }

        [Fact]
        public void FalseWhenCategoryIdCanNotBeEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var categoryIdCanNotBeEmptyRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId);

            // Assert
            categoryIdCanNotBeEmptyRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenCategoryIdCanNotBeEmptyRuleIsBroken()
        {
            // Arrange
            _categoryId = new CategoryId(Guid.Empty);

            // Act
            var categoryIdCanNotBeEmptyRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryIdCanNotBeEmptyRuleIsBroken(_categoryId);

            // Assert
            categoryIdCanNotBeEmptyRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenCategoryNameCanNotBeEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var categoryNameCanNotBeEmptyRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryNameCanNotBeEmptyRuleIsBroken(_categoryName);

            // Assert
            categoryNameCanNotBeEmptyRuleIsBroken.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrueWhenCategoryNameCanNotBeEmptyRuleIsBroken(string categoryName)
        {
            // Arrange
            _categoryName = categoryName;

            // Act
            var categoryNameCanNotBeEmptyRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryNameCanNotBeEmptyRuleIsBroken(_categoryName);

            // Assert
            categoryNameCanNotBeEmptyRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenCategoryNameCanNotBeShorterThanRuleIsNotBroken()
        {
            // Arrange

            // Act
            var categoryNameCanNotBeShorterThanRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryNameCanNotBeShorterThanRuleIsBroken(_categoryName);

            // Assert
            categoryNameCanNotBeShorterThanRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenCategoryNameCanNotBeShorterThanRuleIsBroken()
        {
            // Arrange
            _categoryName = new string('a', GlobalValidationVariables.CategoryNameMinLength - 1);

            // Act
            var categoryNameCanNotBeShorterThanRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryNameCanNotBeShorterThanRuleIsBroken(_categoryName);

            // Assert
            categoryNameCanNotBeShorterThanRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenCategoryNameCanNotBeLongerThanRuleIsNotBroken()
        {
            // Arrange

            // Act
            var categoryNameCanNotBeLongerThanRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryNameCanNotBeLongerThanRuleIsBroken(_categoryName);

            // Assert
            categoryNameCanNotBeLongerThanRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenCategoryNameCanNotBeLongerThanRuleIsBroken()
        {
            // Arrange
            _categoryName = new string('a', GlobalValidationVariables.CategoryNameMaxLength + 1);

            // Act
            var categoryNameCanNotBeLongerThanRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryNameCanNotBeLongerThanRuleIsBroken(_categoryName);

            // Assert
            categoryNameCanNotBeLongerThanRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenCategoryDescriptionCanNotBeEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var categoryDescriptionCanNotBeLongerThanRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeLongerThanRuleIsBroken(_categoryName);

            // Assert
            categoryDescriptionCanNotBeLongerThanRuleIsBroken.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrueWhenCategoryDescriptionCanNotBeEmptyRuleIsBroken(string categoryDescription)
        {
            // Arrange
            _categoryDescription = categoryDescription;

            // Act
            var categoryDescriptionCanNotBeEmptyRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeEmptyRuleIsBroken(_categoryDescription);

            // Assert
            categoryDescriptionCanNotBeEmptyRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void FalseWhenCategoryDescriptionCanNotBeShorterThanRuleIsNotBroken()
        {
            // Arrange

            // Act
            var categoryDescriptionCanNotBeEmptyRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeEmptyRuleIsBroken(_categoryName);

            // Assert
            categoryDescriptionCanNotBeEmptyRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenCategoryDescriptionCanNotBeShorterThanRuleIsBroken()
        {
            // Arrange
            _categoryDescription = new string('a', GlobalValidationVariables.CategoryDescriptionMinLength - 1);

            // Act
            var categoryNameCanNotBeShorterThanRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeShorterThanRuleIsBroken(_categoryDescription);

            // Assert
            categoryNameCanNotBeShorterThanRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenCategoryDescriptionCanNotBeLongerThanRuleIsNotBroken()
        {
            // Arrange

            // Act
            var categoryDescriptionCanNotBeShorterThanRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeShorterThanRuleIsBroken(_categoryDescription);

            // Assert
            categoryDescriptionCanNotBeShorterThanRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenCategoryDescriptionCanNotBeLongerThanRuleIsBroken()
        {
            // Arrange
            _categoryDescription = new string('a', GlobalValidationVariables.CategoryDescriptionMaxLength + 1);

            // Act
            var categoryDescriptionCanNotBeLongerThanRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeLongerThanRuleIsBroken(_categoryDescription);

            // Assert
            categoryDescriptionCanNotBeLongerThanRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenProductPictureCanNotBeNullOrEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var productPictureCanNotBeNullOrEmptyRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(_categoryIcon);

            // Assert
            productPictureCanNotBeNullOrEmptyRuleIsBroken.Should().BeFalse();
        }

        [Theory, MemberData(nameof(Icons))]
        public void TrueWhenProductPictureCanNotBeNullOrEmptyRuleIsBroken(Picture categoryIcon)
        {
            // Arrange
            _categoryIcon = categoryIcon;

            // Act
            var categoryIconCanNotBeNullOrEmptyRuleIsBroken =
                _categoryBusinessRulesChecker.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(_categoryIcon);

            // Assert
            categoryIconCanNotBeNullOrEmptyRuleIsBroken.Should().BeTrue();
        }

        public async Task DisposeAsync()
        {
            _categoryBusinessRulesChecker = null;
            _categoryId = null;
            _categoryName = null;
            _categoryDescription = null;
            _categoryIcon = null;

            await Task.CompletedTask;
        }
    }
}