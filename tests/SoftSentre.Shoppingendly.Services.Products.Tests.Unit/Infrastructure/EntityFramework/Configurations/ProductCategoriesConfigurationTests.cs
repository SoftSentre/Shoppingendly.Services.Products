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
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Configurations
{
    public class ProductCategoriesConfigurationTests
    {
        public ProductCategoriesConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<ProductCategory, ProductCategoriesConfiguration>(
                        new ProductCategoriesConfiguration());
        }

        private readonly EntityTypeBuilder<ProductCategory> _entityTypeBuilder;

        [Fact]
        public void CheckIfCategoryIdHasIsConfiguredAsKeyAndIsRequired()
        {
            // Arrange
            const string categoryId = nameof(ProductCategory.CategoryId);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(categoryId);

            // Act
            var isRequired = !dbProperty.IsNullable;
            var isKey = dbProperty.IsKey();

            // Assert
            isRequired.Should().BeTrue();
            isKey.Should().BeTrue();
        }

        [Fact]
        public void CheckIfProductIdHasIsConfiguredAsKeyAndIsRequired()
        {
            // Arrange
            const string productId = nameof(ProductCategory.ProductId);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(productId);

            // Act
            var isRequired = !dbProperty.IsNullable;
            var isKey = dbProperty.IsKey();

            // Assert
            isRequired.Should().BeTrue();
            isKey.Should().BeTrue();
        }
    }
}