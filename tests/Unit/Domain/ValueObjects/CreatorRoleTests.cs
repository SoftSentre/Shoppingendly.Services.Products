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
using System.Collections.Generic;
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.ValueObjects
{
    public class CreatorRoleTests
    {
        [Fact]
        public void ComparisonBetweenTheSameRolesShouldBeTrue()
        {
            // Assert

            // Act
            var theSame = CreatorRole.Admin.CompareTo(CreatorRole.Admin);
            var notTheSame = CreatorRole.Admin.CompareTo(CreatorRole.Moderator);
            var otherNotTheSame = CreatorRole.Moderator.CompareTo(CreatorRole.Admin);

            // Arrange
            theSame.Should().Be(0);
            notTheSame.Should().Be(-1);
            otherNotTheSame.Should().Be(1);
        }

        [Fact]
        public void ToStringShouldReturnAValueAsString()
        {
            // Assert

            // Act
            var name = CreatorRole.Admin.ToString();

            // Arrange
            name.Should().Be("Admin");
        }

        [Fact]
        public void GetAllShouldReturnAllRoles()
        {
            // Assert

            // Act
            var creatorRoles = Enumeration.GetAll<CreatorRole>();

            // Arrange
            creatorRoles.Should().BeEquivalentTo(new List<CreatorRole>
                {CreatorRole.User, CreatorRole.Moderator, CreatorRole.Admin});
        }

        [Fact]
        public void AbsoluteDifferenceShouldReturnCorrectNumber()
        {
            // Assert

            // Act
            var difference = Enumeration.AbsoluteDifference(CreatorRole.Admin, CreatorRole.User);

            // Arrange
            difference.Should().Be(2);
        }

        [Fact]
        public void FromValueShouldReturnValueBasedOnTheId()
        {
            // Assert

            // Act
            var creatorRole = Enumeration.FromValue<CreatorRole>(3);

            // Arrange
            creatorRole.Should().Be(CreatorRole.User);
        }

        [Fact]
        public void FromDisplayNameShouldReturnIdBasedOnName()
        {
            // Assert

            // Act
            var creatorRole = Enumeration.FromDisplayName<CreatorRole>("Admin");

            // Arrange
            creatorRole.Should().Be(CreatorRole.Admin);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenObjectAreTheSame()
        {
            // Assert
            var creatorRole = CreatorRole.Admin;
            var sameCreatorRole = CreatorRole.Admin;
            var differentCreatorRole = CreatorRole.Moderator;

            // Act
            var theSame = creatorRole.Equals(sameCreatorRole);
            var notTheSame = creatorRole.Equals(differentCreatorRole);
            var notTheSameObjects = creatorRole.Equals(new object());

            // Arrange
            theSame.Should().BeTrue();
            notTheSame.Should().BeFalse();
            notTheSameObjects.Should().BeFalse();
        }

        [Fact]
        public void GetHashCodeShouldReturnTrueWhenObjectAreTheSame()
        {
            // Assert
            var creatorRole = CreatorRole.Admin;
            var sameCreatorRole = CreatorRole.Admin;
            var differentCreatorRole = CreatorRole.Moderator;

            // Act
            var theSame = creatorRole.GetHashCode() == sameCreatorRole.GetHashCode();
            var notTheSame = creatorRole.GetHashCode() == differentCreatorRole.GetHashCode();

            // Arrange
            theSame.Should().BeTrue();
            notTheSame.Should().BeFalse();
        }

        [Fact]
        public void FromDisplayNameShouldThrowWhenNameIsInvalid()
        {
            // Assert
            const string displayName = "not exists";
            
            // Act
            Action fromDisplayName = () => Enumeration.FromDisplayName<CreatorRole>(displayName);

            // Arrange
            fromDisplayName.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"'{displayName}' is not a valid display name in {typeof(CreatorRole)}");
        }
    }
}