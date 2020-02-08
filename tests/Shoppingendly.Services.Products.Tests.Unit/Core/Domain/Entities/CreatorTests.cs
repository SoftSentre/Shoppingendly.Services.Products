using System;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Creators;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Creators;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Entities
{
    public class CreatorTests
    {
        #region domain logic

        [Theory]
        [InlineData("John")]
        [InlineData("My name us too long, but it's in the range, right.")]
        public void CheckIfSetCreatorNameDoNotThrowExceptionWhenCorrectNameHasBeenProvided(string creatorName)
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            Action action = () => creator.SetName(creatorName);

            //Assert
            action.Should().NotThrow<InvalidCreatorNameException>();
        }

        [Fact]
        public void CheckIfSetCreatorNameMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string creatorName = "New creator";
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            creator.SetName(creatorName);

            // Assert
            creator.Name.Should().Be(creatorName);
            creator.UpdatedDate.Should().NotBe(default);
            creator.CreatedAt.Should().NotBe(default);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfSetCreatorNameThrowExceptionWhenNameIsEmptyOrNull(string creatorName)
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            Action action = () => creator.SetName(creatorName);

            //Assert
            action.Should().Throw<InvalidCreatorNameException>()
                .WithMessage("Creator name can not be empty.");
        }

        [Fact]
        public void CheckIfSetCreatorNameThrowExceptionWhenNameIsTooLong()
        {
            // Arrange
            const string creatorName = "Creator name should not be longer than 50 characters.";
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            Action action = () => creator.SetName(creatorName);

            //Assert
            action.Should().Throw<InvalidCreatorNameException>()
                .WithMessage("Creator name can not be longer than 50 characters.");
        }

        [Fact]
        public void CheckIfSetCreatorNameThrowExceptionWhenNameIsTooShort()
        {
            // Arrange
            const string creatorName = "Jo";
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            Action action = () => creator.SetName(creatorName);

            //Assert
            action.Should().Throw<InvalidCreatorNameException>()
                .WithMessage("Creator name can not be shorter than 3 characters.");
        }

        [Fact]
        public void CheckIfSetCreatorEmailDoNotThrowExceptionWhenCorrectEmailHasBeenProvided()
        {
            // Arrange
            const string creatorEmail = "newCreator@email.com";
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            Action action = () => creator.SetEmail(creatorEmail);

            // Assert
            action.Should().NotThrow<InvalidCreatorEmailException>();
        }

        [Fact]
        public void CheckIfSetCreatorEmailMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string creatorEmail = "newCreator@email.com";
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            creator.SetEmail(creatorEmail);

            // Assert
            creator.Email.Should().Be(creatorEmail);
            creator.UpdatedDate.Should().NotBe(default);
            creator.CreatedAt.Should().NotBe(default);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CheckIfSetCreatorEmailThrowExceptionWhenEmailIsNullOrEmpty(string email)
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            Action action = () => creator.SetEmail(email);

            // Assert
            action.Should().Throw<InvalidCreatorEmailException>()
                .WithMessage("Creator email can not be empty.");
        }
        
        [Theory]
        [InlineData("john.email.com")]
        [InlineData("john999@sss")]
        public void CheckIfSetCreatorEmailThrowExceptionWhenEmailDoNotMatchEmailRegex(string email)
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            Action action = () => creator.SetEmail(email);

            // Assert
            action.Should().Throw<InvalidCreatorEmailException>()
                .WithMessage("Invalid email has been provided.");
        }

        [Fact]
        public void CheckIfSetCreatorRoleDoNotThrowExceptionWhenCorrectRoleHasBeenProvided()
        {
            // Arrange
            var creatorRole = Role.Moderator;
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);
            
            // Act
            Action action = () => creator.SetRole(creatorRole);

            //Assert
            action.Should().NotThrow<InvalidCreatorRoleException>();
        }

        [Fact]
        public void CheckIfSetCreatorRoleMethodSetValuesWhenCorrectRoleHasBeenProvided()
        {
            // Arrange
            var creatorRole = Role.Moderator;
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            creator.SetRole(creatorRole);

            // Assert
            creator.Role.Should().Be(creatorRole);
            creator.UpdatedDate.Should().NotBe(default);
            creator.CreatedAt.Should().NotBe(default);
        }
        
        #endregion

        #region domain events

        [Fact]
        public void CheckIfCreateNewCreatorByConstructorProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange

            // Act
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);
            var newCreatorCreatedDomainEvent = creator.GetUncommitted().LastOrDefault() as NewCreatorCreatedDomainEvent;
            
            // Assert
            creator.DomainEvents.Should().NotBeEmpty();
            newCreatorCreatedDomainEvent.Should().BeOfType<NewCreatorCreatedDomainEvent>();
            newCreatorCreatedDomainEvent.Should().NotBeNull();
            newCreatorCreatedDomainEvent.CreatorId.Should().Be(creator.Id);
            newCreatorCreatedDomainEvent.Name.Should().Be(creator.Name);
            newCreatorCreatedDomainEvent.Email.Should().Be(creator.Email);
            newCreatorCreatedDomainEvent.Role.Should().Be(creator.Role);
        }
        
        [Fact]
        public void CheckIfSetCreatorNameMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);
            
            // Act
            creator.SetName("NewCreatorName");
            var creatorNameChangedDomainEvent = creator.GetUncommitted().LastOrDefault() as CreatorNameChangedDomainEvent;
            
            // Assert
            creator.DomainEvents.Should().NotBeEmpty();
            creatorNameChangedDomainEvent.Should().BeOfType<CreatorNameChangedDomainEvent>();
            creatorNameChangedDomainEvent.Should().NotBeNull();
            creatorNameChangedDomainEvent.CreatorId.Should().Be(creator.Id);
            creatorNameChangedDomainEvent.Name.Should().Be(creator.Name);
        }
        
        [Fact]
        public void CheckIfSetCreatorEmailMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);
            
            // Act
            creator.SetEmail("NewCreatorEmail@Email.pl");
            var creatorEmailChangedDomainEvent = creator.GetUncommitted().LastOrDefault() as CreatorEmailChangedDomainEvent;
            
            // Assert
            creator.DomainEvents.Should().NotBeEmpty();
            creatorEmailChangedDomainEvent.Should().BeOfType<CreatorEmailChangedDomainEvent>();
            creatorEmailChangedDomainEvent.Should().NotBeNull();
            creatorEmailChangedDomainEvent.CreatorId.Should().Be(creator.Id);
            creatorEmailChangedDomainEvent.Email.Should().Be(creator.Email);
        }
        
        [Fact]
        public void CheckIfSetCreatorRoleMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);
            
            // Act
            creator.SetRole(Role.User);
            var creatorRoleChangedDomainEvent = creator.GetUncommitted().LastOrDefault() as CreatorRoleChangedDomainEvent;
            
            // Assert
            creator.DomainEvents.Should().NotBeEmpty();
            creatorRoleChangedDomainEvent.Should().BeOfType<CreatorRoleChangedDomainEvent>();
            creatorRoleChangedDomainEvent.Should().NotBeNull();
            creatorRoleChangedDomainEvent.CreatorId.Should().Be(creator.Id);
            creatorRoleChangedDomainEvent.Role.Should().Be(creator.Role);
        }
        
        [Fact]
        public void CheckIfClearDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            creator.ClearDomainEvents();

            // Assert
            creator.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfGetUncommittedDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            var domainEvents = creator.GetUncommitted();
            
            // Assert
            domainEvents.Should().NotBeNull();
            domainEvents.LastOrDefault().Should().BeOfType<NewCreatorCreatedDomainEvent>();
        }

        #endregion
    }
}