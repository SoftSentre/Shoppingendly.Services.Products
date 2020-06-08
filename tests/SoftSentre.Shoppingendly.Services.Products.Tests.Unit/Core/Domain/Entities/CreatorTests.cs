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
using System.Linq;
using FluentAssertions;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Entities
{
    public class CreatorTests
    {
        [Theory]
        [InlineData("John")]
        [InlineData("My name us too long, but it's in the range, right.")]
        public void CheckIfSetCreatorNameDoNotThrowExceptionWhenCorrectNameHasBeenProvided(string creatorName)
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            Action action = () => creator.SetCreatorName(creatorName);

            //Assert
            action.Should().NotThrow<DomainException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfSetCreatorNameThrowExceptionWhenNameIsEmptyOrNull(string creatorName)
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            Action action = () => creator.SetCreatorName(creatorName);

            //Assert
            action.Should().Throw<CreatorNameCanNotBeEmptyException>()
                .WithMessage("Creator name can not be empty.");
        }

        [Fact]
        public void CheckIfClearDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            creator.ClearDomainEvents();

            // Assert
            creator.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfCreateNewCreatorByConstructorProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange

            // Act
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);
            var newCreatorCreatedDomainEvent =
                creator.GetUncommitted().LastOrDefault() as NewCreatorCreatedDomainEvent ??
                It.IsAny<NewCreatorCreatedDomainEvent>();

            // Assert
            creator.DomainEvents.Should().NotBeEmpty();
            newCreatorCreatedDomainEvent.Should().BeOfType<NewCreatorCreatedDomainEvent>();
            newCreatorCreatedDomainEvent.Should().NotBeNull();
            newCreatorCreatedDomainEvent.CreatorId.Should().Be(creator.Id);
            newCreatorCreatedDomainEvent.CreatorName.Should().Be(creator.CreatorName);
            newCreatorCreatedDomainEvent.CreatorRole.Should().Be(creator.CreatorRole);
        }

        [Fact]
        public void CheckIfGetUncommittedDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            var domainEvents = creator.GetUncommitted().ToList();

            // Assert
            domainEvents.Should().NotBeNull();
            domainEvents.LastOrDefault().Should().BeOfType<NewCreatorCreatedDomainEvent>();
        }


        [Fact]
        public void CheckIfSetCreatorNameMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            creator.SetCreatorName("NewCreatorName");
            var creatorNameChangedDomainEvent =
                creator.GetUncommitted().LastOrDefault() as CreatorNameChangedDomainEvent ??
                It.IsAny<CreatorNameChangedDomainEvent>();

            // Assert
            creator.DomainEvents.Should().NotBeEmpty();
            creatorNameChangedDomainEvent.Should().BeOfType<CreatorNameChangedDomainEvent>();
            creatorNameChangedDomainEvent.Should().NotBeNull();
            creatorNameChangedDomainEvent.CreatorId.Should().Be(creator.Id);
            creatorNameChangedDomainEvent.CreatorName.Should().Be(creator.CreatorName);
        }

        [Fact]
        public void CheckIfSetCreatorNameMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string creatorName = "New creator";
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            creator.SetCreatorName(creatorName);

            // Assert
            creator.CreatorName.Should().Be(creatorName);
            creator.UpdatedDate.Should().NotBe(default);
            creator.CreatedAt.Should().NotBe(default);
        }

        [Fact]
        public void CheckIfSetCreatorNameThrowExceptionWhenNameIsTooLong()
        {
            // Arrange
            const string creatorName = "Creator name should not be longer than 50 characters.";
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            Action action = () => creator.SetCreatorName(creatorName);

            //Assert
            action.Should().Throw<CreatorNameIsTooLongException>()
                .WithMessage("Creator name can not be longer than 50 characters.");
        }

        [Fact]
        public void CheckIfSetCreatorNameThrowExceptionWhenNameIsTooShort()
        {
            // Arrange
            const string creatorName = "Jo";
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            Action action = () => creator.SetCreatorName(creatorName);

            //Assert
            action.Should().Throw<CreatorNameIsTooShortException>()
                .WithMessage("Creator name can not be shorter than 3 characters.");
        }

        [Fact]
        public void CheckIfSetCreatorRoleDoNotThrowExceptionWhenCorrectRoleHasBeenProvided()
        {
            // Arrange
            var creatorRole = CreatorRole.Moderator;
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            Action action = () => creator.SetCreatorRole(creatorRole);

            //Assert
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void CheckIfSetCreatorRoleMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            creator.SetCreatorRole(CreatorRole.User);
            var creatorRoleChangedDomainEvent =
                creator.GetUncommitted().LastOrDefault() as CreatorRoleChangedDomainEvent ??
                It.IsAny<CreatorRoleChangedDomainEvent>();

            // Assert
            creator.DomainEvents.Should().NotBeEmpty();
            creatorRoleChangedDomainEvent.Should().BeOfType<CreatorRoleChangedDomainEvent>();
            creatorRoleChangedDomainEvent.Should().NotBeNull();
            creatorRoleChangedDomainEvent.CreatorId.Should().Be(creator.Id);
            creatorRoleChangedDomainEvent.CreatorRole.Should().Be(creator.CreatorRole);
        }

        [Fact]
        public void CheckIfSetCreatorRoleMethodSetValuesWhenCorrectRoleHasBeenProvided()
        {
            // Arrange
            var creatorRole = CreatorRole.Moderator;
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            creator.SetCreatorRole(creatorRole);

            // Assert
            creator.CreatorRole.Should().Be(creatorRole);
            creator.UpdatedDate.Should().NotBe(default);
            creator.CreatedAt.Should().NotBe(default);
        }
    }
}