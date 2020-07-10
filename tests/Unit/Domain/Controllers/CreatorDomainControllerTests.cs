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
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Controllers
{
    public class CreatorDomainControllerTests : IAsyncLifetime
    {
        private Mock<ICreatorRepository> _creatorRepositoryMock;
        private Mock<ICreatorBusinessRulesChecker> _creatorBusinessRulesCheckerMock;
        private Mock<IDomainEventEmitter> _domainEventEmitterMock;
        private CreatorFactory _creatorFactory;
        private Creator _creator;
        private string _creatorName;
        private string _newCreatorName;
        private CreatorId _creatorId;
        private CreatorRole _creatorRole;

        public async Task InitializeAsync()
        {
            _creatorRepositoryMock = new Mock<ICreatorRepository>();
            _creatorBusinessRulesCheckerMock = new Mock<ICreatorBusinessRulesChecker>();
            _domainEventEmitterMock = new Mock<IDomainEventEmitter>();
            _creatorFactory =
                new CreatorFactory(_creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);
            _creatorId = new CreatorId();
            _creatorName = "exampleCreatorName";
            _newCreatorName = "otherExampleCreatorName";
            _creatorRole = CreatorRole.User;
            _creator = _creatorFactory.Create(_creatorId, _creatorName, _creatorRole);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task GetCreatorShouldReturnCorrectResult()
        {
            // Arrange
            _creatorRepositoryMock.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(_creator);

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            var creator = await creatorDomainService.GetCreatorByIdAsync(_creatorId);

            // Assert
            creator.Should().Be(_creator);
            _creatorRepositoryMock.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
        }

        [Fact]
        public async Task GetCreatorWithProductsShouldReturnCorrectResult()
        {
            // Arrange
            _creator.Products.Add(new Product(new ProductId(), _creator.CreatorId, Picture.Empty, "ExampleProductName",
                ProductProducer.Create("ExampleProducer")));

            _creatorRepositoryMock.Setup(cr => cr.GetWithIncludesAsync(_creatorId))
                .ReturnsAsync(_creator);

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            var creator = await creatorDomainService.GetCreatorWithProductsByIdAsync(_creatorId);

            // Assert
            creator.Should().Be(_creator);
            _creatorRepositoryMock.Verify(cr => cr.GetWithIncludesAsync(_creatorId), Times.Once);
        }

        [Fact]
        public async Task CreatorShouldBeSuccessfullyAddedWhenParametersAreCorrect()
        {
            // Arrange
            _creatorRepositoryMock.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(new Maybe<Creator>());

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            var creator = await creatorDomainService.AddNewCreatorAsync(_creatorId, _creatorName, _creatorRole);

            //Assert
            creator.Value.CreatorId.Should().Be(_creatorId);
            creator.Value.CreatorName.Should().Be(_creatorName);
            creator.Value.CreatorRole.Should().Be(_creatorRole);
            creator.Value.CreatedAt.Should().NotBe(default);
            _creatorRepositoryMock.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            _creatorRepositoryMock.Verify(cr => cr.AddAsync(It.IsAny<Creator>()), Times.Once);
            _domainEventEmitterMock.Verify(dve => dve.Emit(creator.Value, It.IsAny<NewCreatorCreatedDomainEvent>()));
        }

        [Fact]
        public void AddCreatorShouldBeFailedWhenCreatorAlreadyExists()
        {
            // Arrange
            _creatorRepositoryMock.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(_creator);

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            Func<Task<Maybe<Creator>>> func = async () =>
                await creatorDomainService.AddNewCreatorAsync(_creatorId, _creatorName, _creatorRole);

            //Assert
            func.Should().Throw<CreatorAlreadyExistsException>()
                .Where(e => e.Code == ErrorCodes.CreatorAlreadyExists)
                .WithMessage($"Unable to add new creator, because creator with id: {_creatorId} is already exists.");

            _creatorRepositoryMock.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            _creatorRepositoryMock.Verify(cr => cr.AddAsync(It.IsAny<Creator>()), Times.Never);
        }

        [Fact]
        public async Task CreatorNameShouldBeSuccessfullyChangedWhenParametersAreCorrect()
        {
            // Arrange
            _creatorRepositoryMock.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(_creator);

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            await creatorDomainService.ChangeCreatorNameAsync(_creatorId, _newCreatorName);

            //Assert
            _creatorRepositoryMock.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            _creatorRepositoryMock.Verify(cr => cr.Update(_creator), Times.Once);
        }

        [Fact]
        public void ChangeCreatorNameShouldBeFailedWhenCreatorNotFound()
        {
            // Arrange
            _creatorRepositoryMock.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(new Maybe<Creator>());

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.ChangeCreatorNameAsync(_creatorId, _newCreatorName);

            //Assert
            func.Should().Throw<CreatorNotFoundException>()
                .Where(e => e.Code == ErrorCodes.CreatorNotFound)
                .WithMessage($"Unable to mutate creator state, because creator with id: {_creatorId} not found.");

            _creatorRepositoryMock.Verify(cr => cr.GetByIdAsync(It.IsAny<CreatorId>()), Times.Once);
            _creatorRepositoryMock.Verify(cr => cr.Update(null), Times.Never);
        }

        [Fact]
        public void ChangeCreatorNameShouldBeFailedWhenCreatorNameIsEmpty()
        {
            ChangeCreatorNameShouldBeFailedWhenCreatorValidationNotPassed(
                checker => checker.CreatorNameCanNotBeEmptyRuleIsBroken(It.IsAny<string>()),
                "Creator name can not be empty.", ErrorCodes.CreatorNameCanNotBeEmpty);
        }

        [Fact]
        public void ChangeCreatorNameShouldBeFailedWhenCreatorNameIsTooShort()
        {
            ChangeCreatorNameShouldBeFailedWhenCreatorValidationNotPassed(
                checker => checker.CreatorNameCanNotBeShorterThanRuleIsBroken(It.IsAny<string>()),
                $"Creator name can not be shorter than {GlobalValidationVariables.CreatorNameMinLength} characters.",
                ErrorCodes.CreatorNameIsTooShort);
        }

        [Fact]
        public void ChangeCreatorNameShouldBeFailedWhenCreatorNameIsTooLong()
        {
            ChangeCreatorNameShouldBeFailedWhenCreatorValidationNotPassed(
                checker => checker.CreatorNameCanNotBeLongerThanRuleIsBroken(It.IsAny<string>()),
                $"Creator name can not be longer than {GlobalValidationVariables.CreatorNameMaxLength} characters.",
                ErrorCodes.CreatorNameIsTooLong);
        }

        [Fact]
        public async Task CreatorRoleShouldBeSuccessfullyChangedWhenParametersAreCorrect()
        {
            // Arrange
            _creatorRepositoryMock.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(_creator);

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            await creatorDomainService.ChangeCreatorRoleAsync(_creatorId, CreatorRole.Moderator);

            //Assert
            _creatorRepositoryMock.Verify(cr => cr.GetByIdAsync(_creatorId), Times.Once);
            _creatorRepositoryMock.Verify(cr => cr.Update(_creator), Times.Once);
        }

        [Fact]
        public void ChangeCreatorROleShouldBeFailedWhenCreatorNotFound()
        {
            // Arrange
            _creatorRepositoryMock.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(new Maybe<Creator>());

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.ChangeCreatorRoleAsync(_creatorId, CreatorRole.Moderator);

            //Assert
            func.Should().Throw<CreatorNotFoundException>()
                .Where(e => e.Code == ErrorCodes.CreatorNotFound)
                .WithMessage($"Unable to mutate creator state, because creator with id: {_creatorId} not found.");

            _creatorRepositoryMock.Verify(cr => cr.GetByIdAsync(It.IsAny<CreatorId>()), Times.Once);
            _creatorRepositoryMock.Verify(cr => cr.Update(null), Times.Never);
        }

        private void ChangeCreatorNameShouldBeFailedWhenCreatorValidationNotPassed(
            Expression<Func<ICreatorBusinessRulesChecker, bool>> brokenRule, string exceptionMessage, string errorCode)
        {
            // Arrange
            _creatorRepositoryMock.Setup(cr => cr.GetByIdAsync(_creatorId))
                .ReturnsAsync(_creator);

            _creatorBusinessRulesCheckerMock.Setup(brokenRule)
                .Returns(true);

            ICreatorDomainController creatorDomainService =
                new CreatorDomainController(_creatorBusinessRulesCheckerMock.Object, _creatorRepositoryMock.Object,
                    _creatorFactory);

            // Act
            Func<Task> func = async () =>
                await creatorDomainService.ChangeCreatorNameAsync(_creatorId, _newCreatorName);

            //Assert
            func.Should().Throw<DomainException>()
                .Where(e => e.Code == errorCode)
                .WithMessage(exceptionMessage);

            _creatorRepositoryMock.Verify(cr => cr.GetByIdAsync(It.IsAny<CreatorId>()), Times.Never);
            _creatorRepositoryMock.Verify(cr => cr.Update(null), Times.Never);
        }

        public async Task DisposeAsync()
        {
            _creatorRepositoryMock = null;
            _creatorBusinessRulesCheckerMock = null;
            _domainEventEmitterMock = null;
            _creatorFactory = null;
            _creatorId = null;
            _creatorName = null;
            _creatorRole = null;

            await Task.CompletedTask;
        }
    }
}