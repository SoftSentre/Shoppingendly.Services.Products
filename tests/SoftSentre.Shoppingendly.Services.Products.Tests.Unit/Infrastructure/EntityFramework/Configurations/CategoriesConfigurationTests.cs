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

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions;
using Xunit;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Configurations
{
    public class CategoriesConfigurationTests
    {
        public CategoriesConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<Category, CategoriesConfiguration>(
                        new CategoriesConfiguration());
        }

        private readonly EntityTypeBuilder<Category> _entityTypeBuilder;

        [Fact]
        public void CheckIfCategoryDescriptionHasConfiguredValidValues()
        {
            // Arrange
            const string description = nameof(Category.CategoryDescription);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(description);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(CategoryDescriptionMaxLength);
            isRequired.Should().Be(IsCategoryDescriptionRequired);
        }

        [Fact]
        public void CheckIfCategoryIdHasIsConfiguredAsKeyAndIsRequired()
        {
            // Arrange
            const string categoryId = nameof(Category.Id);

            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(categoryId);

            // Act
            var isRequired = !dbProperty.IsNullable;
            var isKey = dbProperty.IsKey();

            // Assert
            isRequired.Should().BeTrue();
            isKey.Should().BeTrue();
        }

        [Fact]
        public void CheckIfCategoryNameHasConfiguredValidValues()
        {
            // Arrange
            const string name = nameof(Category.CategoryName);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(name);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(CategoryNameMaxLength);
            isRequired.Should().Be(IsCategoryNameRequired);
        }
        
        [Fact]
        public void CheckIfProductPictureNameHasConfiguredValidValues()
        {
            // Arrange
            const string categoryIcon = nameof(Category.CategoryIcon);
            const string categoryIconName = nameof(Picture.Name);

            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredNavigation(categoryIcon)
                .GetTargetType()
                .FindDeclaredProperty(categoryIconName);

            // Act
            var maxLength = dbProperty.GetMaxLength();

            // Assert
            maxLength.Should().Be(PictureNameMaxLength);
        }

        [Fact]
        public void CheckIfProductPictureUrlHasConfiguredValidValues()
        {
            // Arrange
            const string categoryIcon = nameof(Category.CategoryIcon);
            const string categoryIconUrl = nameof(Picture.Url);

            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredNavigation(categoryIcon)
                .GetTargetType()
                .FindDeclaredProperty(categoryIconUrl);

            // Act
            var maxLength = dbProperty.GetMaxLength();

            // Assert
            maxLength.Should().Be(PictureUrlMaxLength);
        }

        [Fact]
        public void CheckOtherValuesFromCategoryEntity()
        {
            // Arrange
            const string updatedDate = nameof(Category.UpdatedDate);
            const string createdDate = nameof(Category.CreatedAt);

            var updatedDateDbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(updatedDate);
            var createdDateDbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(createdDate);

            // Act
            var isUpdatedDateRequired = !updatedDateDbProperty.IsNullable;
            var isCreatedDateRequired = !createdDateDbProperty.IsNullable;

            // Assert
            isUpdatedDateRequired.Should().BeFalse();
            isCreatedDateRequired.Should().BeTrue();
        }
    }
}