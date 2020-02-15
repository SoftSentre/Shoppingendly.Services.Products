using System;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Services;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services;
using Shoppingendly.Services.Products.Core.Types;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Services
{
    public class CreatorDomainServiceTests
    {
        [Fact]
        public void CheckIfAddNewCreatorMethodCreateValidObjectAndDoNotThrown()
        {
            // Arrange
            var creatorId = new CreatorId();
            const string creatorName = "ExampleCreatorName";
            const string creatorEmail = "exampleCreator@email.com";
            var creatorRole = Role.Admin;
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Func<Maybe<Creator>> func = () =>
                creatorDomainService.AddNewCreator(creatorId, creatorName, creatorEmail, creatorRole);

            //Assert
            func.Should().NotThrow();
            var testResult = func.Invoke();
            testResult.Value.Id.Should().Be(creatorId);
            testResult.Value.Name.Should().Be(creatorName);
            testResult.Value.Email.Should().Be(creatorEmail);
            testResult.Value.Role.Should().Be(creatorRole);
            testResult.Value.CreatedAt.Should().NotBe(default);
        }
        
        [Fact]
        public void CheckIfChangeCreatorNameMethodSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newCreatorName = "OtherExampleCreatorName";
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorName(creator, newCreatorName);
            action.Invoke();
            
            //Assert
            creator.Name.Should().Be("OtherExampleCreatorName");
        }

        [Fact]
        public void CheckIfChangeCreatorNameMethodDoNotThrownAnyException()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorName(creator, "OtherExampleCreatorName");

            //Assert
            action.Should().NotThrow();
        }
        
        [Fact]
        public void CheckIfSetCreatorNameMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorName(null, "OtherExampleCreatorName");

            //Assert
            action.Should().Throw<EmptyCreatorProvidedException>()
                .WithMessage("Unable to mutate creator state, because provided value is empty.");
        }

        [Fact]
        public void CheckIfChangeCreatorEmailMethodSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newCreatorEmail = "otherCreatorEmail@email.com";
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorEmail(creator, newCreatorEmail);
            action.Invoke();
            
            //Assert
            creator.Email.Should().Be("otherCreatorEmail@email.com");
        }
        
        [Fact]
        public void CheckIfChangeCreatorEmailMethodDoNotThrownAnyException()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorEmail(creator, "otherCreator@email.com");

            //Assert
            action.Should().NotThrow();
        }
        
        [Fact]
        public void CheckIfSetCreatorEmailMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorEmail(null, "otherEmail@email.com");

            //Assert
            action.Should().Throw<EmptyCreatorProvidedException>()
                .WithMessage("Unable to mutate creator state, because provided value is empty.");
        }
        
        [Fact]
        public void CheckIfChangeCreatorRoleMethodSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            var newCreatorRole = Role.Moderator;
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorRole(creator, newCreatorRole);
            action.Invoke();
            
            //Assert
            creator.Role.Should().Be(Role.Moderator);
        }

        [Fact]
        public void CheckIfChangeCreatorRoleMethodDoNotThrownAnyException()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorRole(creator, Role.Moderator);

            //Assert
            action.Should().NotThrow();
        }
        
        [Fact]
        public void CheckIfSetCreatorRoleMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            ICreatorDomainService creatorDomainService = new CreatorDomainService();

            // Act
            Action action = () => creatorDomainService.SetCreatorRole(null, Role.Admin);

            //Assert
            action.Should().Throw<EmptyCreatorProvidedException>()
                .WithMessage("Unable to mutate creator state, because provided value is empty.");
        }
    }
}