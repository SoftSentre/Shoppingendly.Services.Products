using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions;
using Xunit;
using static Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Configurations
{
    public class CategoriesConfigurationTests
    {
        private readonly EntityTypeBuilder<Category> _entityTypeBuilder;

        public CategoriesConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<Category, CategoriesConfiguration>(
                        new CategoriesConfiguration());
        }

        [Fact]
        public void CheckIfCategoryNameHasConfiguredValidValues()
        {
            // Arrange
            const string name = nameof(Category.Name);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(name);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(CategoryNameMaxLength);
            isRequired.Should().Be(IsCategoryNameRequired);
        }

        [Fact]
        public void CheckIfCategoryDescriptionHasConfiguredValidValues()
        {
            // Arrange
            const string description = nameof(Category.Description);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(description);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(CategoryDescriptionMaxLength);
            isRequired.Should().Be(IsCategoryDescriptionRequired);
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
    }
}