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
using System.Threading.Tasks;
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Services
{
    public class CreatorBusinessRulesCheckerTests : IAsyncLifetime
    {
        private ICreatorBusinessRulesChecker _creatorBusinessRulesChecker;

        private CreatorId _creatorId;
        private string _creatorName;

        public async Task InitializeAsync()
        {
            _creatorBusinessRulesChecker = new CreatorBusinessRulesChecker();
            _creatorId = new CreatorId(new Guid("BFF2E985-8C43-4B20-86BF-A43A26B5012D"));
            _creatorName = "exampleCreator";

            await Task.CompletedTask;
        }

        [Fact]
        public void FalseWhenCreatorIdCanNotBeEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var creatorIdCanNotBeEmptyRuleIsBroken =
                _creatorBusinessRulesChecker.CreatorIdCanNotBeEmptyRuleIsBroken(_creatorId);

            // Assert
            creatorIdCanNotBeEmptyRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenCreatorIdCanNotBeEmptyRuleIsBroken()
        {
            // Arrange
            _creatorId = new CreatorId(Guid.Empty);

            // Act
            var creatorIdCanNotBeEmptyRuleIsBroken =
                _creatorBusinessRulesChecker.CreatorIdCanNotBeEmptyRuleIsBroken(_creatorId);

            // Assert
            creatorIdCanNotBeEmptyRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenCreatorNameCanNotBeEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var creatorIdCanNotBeEmptyRuleIsBroken =
                _creatorBusinessRulesChecker.CreatorNameCanNotBeEmptyRuleIsBroken(_creatorName);

            // Assert
            creatorIdCanNotBeEmptyRuleIsBroken.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrueWhenCreatorNameCanNotBeEmptyRuleIsBroken(string creatorName)
        {
            // Arrange
            _creatorName = creatorName;

            // Act
            var creatorNameCanNotBeEmptyRuleIsBroken =
                _creatorBusinessRulesChecker.CreatorNameCanNotBeEmptyRuleIsBroken(_creatorName);

            // Assert
            creatorNameCanNotBeEmptyRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseCreatorNameCanNotBeShorterThanRuleIsNotBroken()
        {
            // Arrange

            // Act
            var creatorNameCanNotBeShorterThanRuleIsBroken =
                _creatorBusinessRulesChecker.CreatorNameCanNotBeShorterThanRuleIsBroken(_creatorName);

            // Assert
            creatorNameCanNotBeShorterThanRuleIsBroken.Should().BeFalse();
        }
        
        [Fact]
        public void TrueCreatorNameCanNotBeShorterThanRuleIsBroken()
        {
            // Arrange
            _creatorName = new string('a', GlobalValidationVariables.CreatorNameMinLength - 1);
            
            // Act
            var creatorNameCanNotBeShorterThanRuleIsBroken =
                _creatorBusinessRulesChecker.CreatorNameCanNotBeShorterThanRuleIsBroken(_creatorName);

            // Assert
            creatorNameCanNotBeShorterThanRuleIsBroken.Should().BeTrue();
        }
        
        [Fact]
        public void FalseCreatorNameCanNotBeLongerThanRuleIsNotBroken()
        {
            // Arrange

            // Act
            var creatorNameCanNotBeLongerThanRuleIsBroken =
                _creatorBusinessRulesChecker.CreatorNameCanNotBeLongerThanRuleIsBroken(_creatorName);
            
            // Assert
            creatorNameCanNotBeLongerThanRuleIsBroken.Should().BeFalse();
        }
        
        [Fact]
        public void TrueCreatorNameCanNotBeLongerThanRuleIsBroken()
        {
            // Arrange
            _creatorName = new string('a', GlobalValidationVariables.CreatorNameMaxLength + 1);
            
            // Act
            var creatorNameCanNotBeLongerThanRuleIsBroken =
                _creatorBusinessRulesChecker.CreatorNameCanNotBeLongerThanRuleIsBroken(_creatorName);

            // Assert
            creatorNameCanNotBeLongerThanRuleIsBroken.Should().BeTrue();
        }
        
        public async Task DisposeAsync()
        {
            _creatorBusinessRulesChecker = null;
            _creatorId = null;
            _creatorName = null;

            await Task.CompletedTask;
        }
    }
}