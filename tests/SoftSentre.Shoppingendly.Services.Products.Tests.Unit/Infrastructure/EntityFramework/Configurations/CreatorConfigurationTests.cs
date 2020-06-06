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
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions;
using Xunit;
using static SoftSentre.Shoppingendly.Services.Products.Globals.Validation.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Configurations
{
    public class CreatorConfigurationTests
    {
        public CreatorConfigurationTests()
        {
            _entityTypeBuilder =
                ConfigurationMetadataTestsExtensions
                    .GetCustomerEntityConfigurationMetadata<Creator, CreatorsConfiguration>(
                        new CreatorsConfiguration());
        }

        private readonly EntityTypeBuilder<Creator> _entityTypeBuilder;

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
    }
}