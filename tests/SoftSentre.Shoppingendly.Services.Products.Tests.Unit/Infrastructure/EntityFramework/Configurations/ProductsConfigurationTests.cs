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
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions;
using Xunit;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Configurations
{
    public class ProductsConfigurationTests
    {
        public ProductsConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<Product, ProductsConfiguration>(
                        new ProductsConfiguration());
        }

        private readonly EntityTypeBuilder<Product> _entityTypeBuilder;

        [Fact]
        public void CheckIfProductIdHasIsConfiguredAsKeyAndIsRequired()
        {
            // Arrange
            const string productId = nameof(Product.Id);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(productId);

            // Act
            var isRequired = !dbProperty.IsNullable;
            var isKey = dbProperty.IsKey();

            // Assert
            isRequired.Should().BeTrue();
            isKey.Should().BeTrue();
        }

        [Fact]
        public void CheckIfProductNameHasConfiguredValidValues()
        {
            // Arrange
            const string name = nameof(Product.ProductName);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(name);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(ProductNameMaxLength);
            isRequired.Should().Be(IsProductNameRequired);
        }

        [Fact]
        public void CheckIfProductPictureNameHasConfiguredValidValues()
        {
            // Arrange
            const string picture = nameof(Product.ProductPicture);
            const string pictureName = nameof(Picture.Name);

            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredNavigation(picture)
                .GetTargetType()
                .FindDeclaredProperty(pictureName);

            // Act
            var maxLength = dbProperty.GetMaxLength();

            // Assert
            maxLength.Should().Be(PictureNameMaxLength);
        }

        [Fact]
        public void CheckIfProductPictureUrlHasConfiguredValidValues()
        {
            // Arrange
            const string picture = nameof(Product.ProductPicture);
            const string pictureUrl = nameof(Picture.Url);

            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredNavigation(picture)
                .GetTargetType()
                .FindDeclaredProperty(pictureUrl);

            // Act
            var maxLength = dbProperty.GetMaxLength();

            // Assert
            maxLength.Should().Be(PictureUrlMaxLength);
        }

        [Fact]
        public void CheckIfProductProducerHasConfiguredValidValues()
        {
            // Arrange
            const string producer = nameof(Product.Producer);
            const string producerName = nameof(Product.Producer.Name);

            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredNavigation(producer)
                .GetTargetType()
                .FindDeclaredProperty(producerName);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(ProductProducerMaxLength);
            isRequired.Should().Be(IsProductProducerRequired);
        }

        [Fact]
        public void CheckOtherValuesFromProductEntity()
        {
            // Arrange
            const string creatorId = nameof(Product.CreatorId);
            const string updatedDate = nameof(Product.UpdatedDate);
            const string createdDate = nameof(Product.CreatedAt);
            var creatorIdDbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(creatorId);
            var updatedDateDbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(updatedDate);
            var createdDateDbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(createdDate);

            // Act
            var isCreatorIdRequired = !creatorIdDbProperty.IsNullable;
            var isUpdatedDateRequired = !updatedDateDbProperty.IsNullable;
            var isCreatedDateRequired = !createdDateDbProperty.IsNullable;

            // Assert
            isCreatorIdRequired.Should().BeTrue();
            isUpdatedDateRequired.Should().BeFalse();
            isCreatedDateRequired.Should().BeTrue();
        }
    }
}