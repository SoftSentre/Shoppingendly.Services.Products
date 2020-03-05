using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Configurations
{
    public class ProductCategoriesConfigurationTests
    {
        private readonly EntityTypeBuilder<ProductCategory> _entityTypeBuilder;

        public ProductCategoriesConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<ProductCategory, ProductCategoriesConfiguration>(
                        new ProductCategoriesConfiguration());
        }

        [Fact]
        public void CheckIfProductIdHasIsConfiguredAsKeyAndIsRequired()
        {
            // Arrange
            const string productId = nameof(ProductCategory.FirstKey);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(productId);

            // Act
            var isRequired = !dbProperty.IsNullable;
            var isKey = dbProperty.IsKey();

            // Assert
            isRequired.Should().BeTrue();
            isKey.Should().BeTrue();
        }

        [Fact]
        public void CheckIfCategoryIdHasIsConfiguredAsKeyAndIsRequired()
        {
            // Arrange
            const string categoryId = nameof(ProductCategory.SecondKey);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(categoryId);

            // Act
            var isRequired = !dbProperty.IsNullable;
            var isKey = dbProperty.IsKey();

            // Assert
            isRequired.Should().BeTrue();
            isKey.Should().BeTrue();
        }
    }
}