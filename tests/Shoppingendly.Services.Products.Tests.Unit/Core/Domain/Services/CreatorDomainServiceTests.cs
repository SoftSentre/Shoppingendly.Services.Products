using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.Services;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services.Creators;
using Shoppingendly.Services.Products.Core.Types;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Services
{
    public class CreatorDomainServiceTests
    {
        private const string CreatorName = "ExampleCreatorName";
        private const string CreatorEmail = "exampleCreator@email.com";

        private readonly CreatorId _creatorId;
        private readonly Role _creatorRole;
        private readonly Creator _creator;

        public CreatorDomainServiceTests()
        {
            _creatorId = new CreatorId();
            _creatorRole = Role.User;
            _creator = Creator.Create(_creatorId, CreatorName, CreatorEmail, _creatorRole);
        }

        [Fact]
        public async Task CheckIfGetCreatorMethodReturnValidObject()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(_creator);

            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            var testResult = await creatorDomainService.GetCreatorAsync(_creatorId);

            // Assert
            testResult.Should().Be(_creator);
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetCreatorByNameMethodReturnValidObject()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByNameAsync(CreatorName))
                .ReturnsAsync(_creator);

            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            var testResult = await creatorDomainService.GetCreatorByNameAsync(CreatorName);

            // Assert
            testResult.Should().Be(_creator);
            creatorRepository.Verify(cr => cr.GetByNameAsync(CreatorName), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetCreatorWithProductsMethodReturnValidObject()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            var creator = new Creator(new CreatorId(), CreatorName, CreatorEmail, Role.User);
            creator.Products.Add(new Product(new ProductId(), creator.Id, Picture.Empty, "ExampleProductName",
                "ExampleProducer"));

            creatorRepository.Setup(cr => cr.GetWithIncludesAsync(CreatorName))
                .ReturnsAsync(creator);

            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            var testResult = await creatorDomainService.GetCreatorWithProductsAsync(CreatorName);

            // Assert
            testResult.Should().Be(creator);
            creatorRepository.Verify(cr => cr.GetWithIncludesAsync(CreatorName), Times.Once);
        }


        [Fact]
        public async Task CheckIfAddNewCreatorMethodCreateValidObject()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(new Maybe<Creator>());

            // Act
            var testResult =
                await creatorDomainService.AddNewCreatorAsync(_creatorId, CreatorName, CreatorEmail, _creatorRole);

            //Assert
            testResult.Value.Id.Should().Be(_creatorId);
            testResult.Value.Name.Should().Be(CreatorName);
            testResult.Value.Email.Should().Be(CreatorEmail);
            testResult.Value.Role.Should().Be(_creatorRole);
            testResult.Value.CreatedAt.Should().NotBe(default);
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.AddAsync(It.IsAny<Creator>()),
                Times.Once);
        }

        [Fact]
        public void CheckIfAddNewCreatorMethodDoNotThrowException()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task<Maybe<Creator>>> func = async () =>
                await creatorDomainService.AddNewCreatorAsync(_creatorId, CreatorName, CreatorEmail, _creatorRole);

            //Assert
            func.Should().NotThrow();
            creatorRepository.Verify(cr => cr.AddAsync(It.IsAny<Creator>()),
                Times.Once);
        }

        [Fact]
        public void CheckIfAddNewCreatorMethodThrowExceptionWhenCreatorAlreadyExists()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(_creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task<Maybe<Creator>>> func = async () =>
                await creatorDomainService.AddNewCreatorAsync(_creatorId, CreatorName, CreatorEmail, _creatorRole);

            //Assert
            func.Should().Throw<CreatorAlreadyExistsException>()
                .WithMessage($"Unable to add new creator, because creator with id: {_creatorId} is already exists.");
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.AddAsync(It.IsAny<Creator>()), Times.Never);
        }

        [Fact]
        public void CheckIfChangeCreatorNameMethodSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newCreatorName = "OtherExampleCreatorName";
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Action action = async () => await creatorDomainService.SetCreatorNameAsync(_creatorId, newCreatorName);
            action.Invoke();

            //Assert
            creator.Name.Should().Be("OtherExampleCreatorName");
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
        }

        [Fact]
        public void CheckIfChangeCreatorNameMethodDoNotThrownAnyException()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.SetCreatorNameAsync(_creatorId, "OtherExampleCreatorName");

            //Assert
            func.Should().NotThrow();
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
        }

        [Fact]
        public void CheckIfSetCreatorNameMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync((Creator) null);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.SetCreatorNameAsync(_creatorId, "OtherExampleCreatorName");

            //Assert
            func.Should().Throw<CreatorNotFoundException>()
                .WithMessage($"Unable to mutate creator state, because creator with id: {_creatorId} not found.");
            creatorRepository.Verify(cr => cr.GetByIdAsync(It.IsAny<CreatorId>()), Times.Once);
            creatorRepository.Verify(cr => cr.Update(null), Times.Never);
        }


        [Fact]
        public void CheckIfChangeCreatorEmailMethodSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newCreatorEmail = "otherCreatorEmail@email.com";
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Action action = async () => await creatorDomainService.SetCreatorEmailAsync(_creatorId, newCreatorEmail);
            action.Invoke();

            //Assert
            creator.Email.Should().Be("otherCreatorEmail@email.com");
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
        }

        [Fact]
        public void CheckIfChangeCreatorEmailMethodDoNotThrownAnyException()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.SetCreatorEmailAsync(_creatorId, "otherCreator@email.com");

            //Assert
            func.Should().NotThrow();
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
        }

        [Fact]
        public void CheckIfSetCreatorEmailMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync((Creator) null);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.SetCreatorEmailAsync(_creatorId, "otherCreator@email.com");

            //Assert
            func.Should().Throw<CreatorNotFoundException>()
                .WithMessage($"Unable to mutate creator state, because creator with id: {_creatorId} not found.");
            creatorRepository.Verify(cr => cr.GetByIdAsync(It.IsAny<CreatorId>()), Times.Once);
            creatorRepository.Verify(cr => cr.Update(null), Times.Never);
        }

        [Fact]
        public void CheckIfChangeCreatorRoleMethodSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Action action = async () => await creatorDomainService.SetCreatorRoleAsync(_creatorId, Role.Admin);
            action.Invoke();

            //Assert
            creator.Role.Should().Be(Role.Admin);
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
        }

        [Fact]
        public void CheckIfChangeCreatorRoleMethodDoNotThrownAnyException()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", "creator@email.com", Role.User);
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.SetCreatorRoleAsync(_creatorId, Role.Admin);

            //Assert
            func.Should().NotThrow();
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
        }

        [Fact]
        public void CheckIfSetCreatorRoleMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync((Creator) null);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.SetCreatorRoleAsync(_creatorId, Role.Admin);

            //Assert
            func.Should().Throw<CreatorNotFoundException>()
                .WithMessage($"Unable to mutate creator state, because creator with id: {_creatorId} not found.");
            creatorRepository.Verify(cr => cr.GetByIdAsync(It.IsAny<CreatorId>()), Times.Once);
            creatorRepository.Verify(cr => cr.Update(null), Times.Never);
        }
    }
}