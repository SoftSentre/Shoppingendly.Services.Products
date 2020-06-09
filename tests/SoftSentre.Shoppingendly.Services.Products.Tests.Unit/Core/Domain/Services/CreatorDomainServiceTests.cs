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
using Moq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Services
{
    public class CreatorDomainServiceTests
    {
        public CreatorDomainServiceTests()
        {
            _creatorId = new CreatorId();
            _creatorCreatorRole = CreatorRole.User;
            _creator = Creator.Create(_creatorId, CreatorName, _creatorCreatorRole);
        }

        private const string CreatorName = "ExampleCreatorName";

        private readonly CreatorId _creatorId;
        private readonly CreatorRole _creatorCreatorRole;
        private readonly Creator _creator;


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
                await creatorDomainService.AddNewCreatorAsync(_creatorId, CreatorName, _creatorCreatorRole);

            //Assert
            testResult.Value.Id.Should().Be(_creatorId);
            testResult.Value.CreatorName.Should().Be(CreatorName);
            testResult.Value.CreatorRole.Should().Be(_creatorCreatorRole);
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
                await creatorDomainService.AddNewCreatorAsync(_creatorId, CreatorName, _creatorCreatorRole);

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
                await creatorDomainService.AddNewCreatorAsync(_creatorId, CreatorName, _creatorCreatorRole);

            //Assert
            func.Should().Throw<CreatorAlreadyExistsException>()
                .WithMessage($"Unable to add new creator, because creator with id: {_creatorId} is already exists.");
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.AddAsync(It.IsAny<Creator>()), Times.Never);
        }

        [Fact]
        public void CheckIfChangeCreatorNameMethodDoNotThrownAnyException()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", CreatorRole.User);
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
        public void CheckIfChangeCreatorNameMethodSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            const string newCreatorName = "OtherExampleCreatorName";
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", CreatorRole.User);
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Action action = async () => await creatorDomainService.SetCreatorNameAsync(_creatorId, newCreatorName);
            action.Invoke();

            //Assert
            creator.CreatorName.Should().Be("OtherExampleCreatorName");
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
        }

        [Fact]
        public void CheckIfChangeCreatorRoleMethodDoNotThrownAnyException()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", CreatorRole.User);
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.SetCreatorRoleAsync(_creatorId, CreatorRole.Admin);

            //Assert
            func.Should().NotThrow();
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
        }

        [Fact]
        public void CheckIfChangeCreatorRoleMethodSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "ExampleCreatorName", CreatorRole.User);
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(creator);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Action action = async () => await creatorDomainService.SetCreatorRoleAsync(_creatorId, CreatorRole.Admin);
            action.Invoke();

            //Assert
            creator.CreatorRole.Should().Be(CreatorRole.Admin);
            creatorRepository.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            creatorRepository.Verify(cr => cr.Update(creator), Times.Once);
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
        public async Task CheckIfGetCreatorWithProductsMethodReturnValidObject()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            var creator = new Creator(new CreatorId(), CreatorName, CreatorRole.User);
            creator.Products.Add(new Product(new ProductId(), creator.Id, ProductPicture.Empty, "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer")));

            creatorRepository.Setup(cr => cr.GetWithIncludesAsync(_creatorId))
                .ReturnsAsync(creator);

            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            var testResult = await creatorDomainService.GetCreatorWithProductsAsync(_creatorId);

            // Assert
            testResult.Should().Be(creator);
            creatorRepository.Verify(cr => cr.GetWithIncludesAsync(_creatorId), Times.Once);
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
        public void CheckIfSetCreatorRoleMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var creatorRepository = new Mock<ICreatorRepository>();
            creatorRepository.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync((Creator) null);
            ICreatorDomainService creatorDomainService = new CreatorDomainService(creatorRepository.Object);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.SetCreatorRoleAsync(_creatorId, CreatorRole.Admin);

            //Assert
            func.Should().Throw<CreatorNotFoundException>()
                .WithMessage($"Unable to mutate creator state, because creator with id: {_creatorId} not found.");
            creatorRepository.Verify(cr => cr.GetByIdAsync(It.IsAny<CreatorId>()), Times.Once);
            creatorRepository.Verify(cr => cr.Update(null), Times.Never);
        }
    }
}