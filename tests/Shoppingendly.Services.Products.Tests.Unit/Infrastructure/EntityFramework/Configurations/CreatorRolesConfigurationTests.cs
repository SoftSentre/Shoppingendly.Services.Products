using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions;
using Xunit;
using static Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Configurations
{
    public class CreatorRolesConfigurationTests
    {
        private readonly EntityTypeBuilder<Role> _entityTypeBuilder;

        public CreatorRolesConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<Role, CreatorRolesConfiguration>(
                        new CreatorRolesConfiguration());
        }

        [Fact]
        public void CheckIfRoleNameHasConfiguredValidValues()
        {
            // Arrange
            const string name = nameof(Role.Name);
            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(name);

            // Act
            var maxLength = dbProperty.GetMaxLength();
            var isRequired = !dbProperty.IsNullable;

            // Assert
            maxLength.Should().Be(RoleNameMaxLength);
            isRequired.Should().BeTrue();
        }

        [Fact]
        public void CheckIfRoleIdHasIsConfiguredAsKeyAndIsRequired()
        {
            // Arrange
            const string roleId = nameof(Role.Id);

            var dbProperty = _entityTypeBuilder.Metadata.FindDeclaredProperty(roleId);

            // Act
            var isRequired = !dbProperty.IsNullable;
            var isKey = dbProperty.IsKey();

            // Assert
            isRequired.Should().BeTrue();
            isKey.Should().BeTrue();
        }
    }
}