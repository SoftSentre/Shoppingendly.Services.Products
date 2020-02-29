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
    public class CreatorConfigurationTests
    {
        private readonly EntityTypeBuilder<Creator> _entityTypeBuilder;

        public CreatorConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<Creator, CreatorsConfiguration>(
                        new CreatorsConfiguration());
        }

        [Fact]
        public void CheckIfCreatorNameHasConfiguredValidValues()
        {
            // Arrange
            const string name = nameof(Creator.Name);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(name);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(CreatorNameMaxLength);
            isRequired.Should().Be(IsCategoryNameRequired);
        }

        [Fact]
        public void CheckIfCreatorEmailHasConfiguredValidValues()
        {
            // Arrange
            const string email = nameof(Creator.Email);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(email);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(CreatorEmailMaxLength);
            isRequired.Should().Be(IsCreatorEmailRequired);
        }

        [Fact]
        public void CheckOtherValuesFromCreatorEntity()
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
        public void CheckIfCreatorIdHasIsConfiguredAsKeyAndIsRequired()
        {
            // Arrange
            const string creatorId = nameof(Creator.Id);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(creatorId);

            // Act
            var isRequired = !dbProperty.IsNullable;
            var isKey = dbProperty.IsKey();

            // Assert
            isRequired.Should().BeTrue();
            isKey.Should().BeTrue();
        }
    }
}