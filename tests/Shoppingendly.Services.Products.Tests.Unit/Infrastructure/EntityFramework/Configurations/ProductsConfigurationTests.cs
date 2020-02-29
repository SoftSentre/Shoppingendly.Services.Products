using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions;
using Xunit;
using static Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Configurations
{
    public class ProductsConfigurationTests
    {
        private readonly EntityTypeBuilder<Product> _entityTypeBuilder;

        public ProductsConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<Product, ProductsConfiguration>(
                        new ProductsConfiguration());
        }

        [Fact]
        public void CheckIfProductNameHasConfiguredValidValues()
        {
            // Arrange
            const string name = nameof(Product.Name);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(name);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(ProductNameMaxLength);
            isRequired.Should().Be(IsProductNameRequired);
        }

        [Fact]
        public void CheckIfProductProducerHasConfiguredValidValues()
        {
            // Arrange
            const string producer = nameof(Product.Producer);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(producer);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(ProductProducerMaxLength);
            isRequired.Should().Be(IsProductProducerRequired);
        }

        [Fact]
        public void CheckIfProductPictureNameHasConfiguredValidValues()
        {
            // Arrange
            const string picture = nameof(Product.Picture);
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
            const string picture = nameof(Product.Picture);
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
    }
}