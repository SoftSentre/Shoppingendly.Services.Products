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
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects.StronglyTypedIds;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Factories
{
    public class CreatorFactoryTests : IAsyncLifetime
    {
        private Mock<ICreatorBusinessRulesChecker> _creatorBusinessRulesCheckerMock;
        private Mock<IDomainEventEmitter> _domainEventEmitterMock;
        private CreatorFactory _creatorFactory;

        private CreatorId _creatorId;
        private string _creatorName;
        private CreatorRole _creatorRole;

        public async Task InitializeAsync()
        {
            _creatorBusinessRulesCheckerMock = new Mock<ICreatorBusinessRulesChecker>();
            _domainEventEmitterMock = new Mock<IDomainEventEmitter>();
            _creatorId = new CreatorId(new Guid("4002E1F0-ED14-48A4-A6DC-DBED0491B06F"));
            _creatorName = "exampleCreatorName";
            _creatorRole = CreatorRole.User;

            await Task.CompletedTask;
        }

        private void FailToCreateCreatorWhenRuleIsBroken<T>(
            Expression<Func<ICreatorBusinessRulesChecker, bool>> brokenRule, T exception) where T : DomainException
        {
            // Act
            _creatorBusinessRulesCheckerMock.Setup(brokenRule)
                .Returns(true);

            _creatorFactory =
                new CreatorFactory(_creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Func<Creator> createCreator = () => _creatorFactory.Create(_creatorId, _creatorName, _creatorRole);

            // Assert
            createCreator.Should().Throw<T>()
                .Where(e => e.Code == exception.Code)
                .WithMessage(exception.Message);

            _creatorBusinessRulesCheckerMock.Verify(brokenRule, Times.Once);
            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Creator>(), It.IsAny<NewCreatorCreatedDomainEvent>()),
                Times.Never);
        }

        public async Task DisposeAsync()
        {
            _creatorBusinessRulesCheckerMock = null;
            _domainEventEmitterMock = null;
            _creatorId = null;
            _creatorName = null;
            _creatorRole = null;

            await Task.CompletedTask;
        }

        [Fact]
        public void CreateCreatorShouldNotRiseAnyExceptionWhenParametersAreCorrect()
        {
            // Act
            _creatorFactory =
                new CreatorFactory(_creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            Func<Creator> createCreator = () => _creatorFactory.Create(_creatorId, _creatorName, _creatorRole);

            // Assert
            createCreator.Should().NotThrow<DomainException>();
        }

        [Fact]
        public void FailToCreateCreatorWhenCreatorIdIsEmpty()
        {
            FailToCreateCreatorWhenRuleIsBroken(checker =>
                    checker.CreatorIdCanNotBeEmptyRuleIsBroken(It.IsAny<CreatorId>()),
                new InvalidCreatorIdException(_creatorId));
        }

        [Fact]
        public void FailToCreateCreatorWhenCreatorNameIsEmpty()
        {
            FailToCreateCreatorWhenRuleIsBroken(checker =>
                    checker.CreatorNameCanNotBeEmptyRuleIsBroken(It.IsAny<string>()),
                new CreatorNameCanNotBeEmptyException());
        }

        [Fact]
        public void FailToCreateCreatorWhenCreatorNameIsTooLong()
        {
            FailToCreateCreatorWhenRuleIsBroken(checker =>
                    checker.CreatorNameCanNotBeLongerThanRuleIsBroken(It.IsAny<string>()),
                new CreatorNameIsTooLongException(GlobalValidationVariables.CreatorNameMaxLength));
        }

        [Fact]
        public void FailToCreateCreatorWhenCreatorNameIsTooShort()
        {
            FailToCreateCreatorWhenRuleIsBroken(checker =>
                    checker.CreatorNameCanNotBeShorterThanRuleIsBroken(It.IsAny<string>()),
                new CreatorNameIsTooShortException(GlobalValidationVariables.CreatorNameMinLength));
        }

        [Fact]
        public void SuccessToCreateCreatorWhenParametersAreCorrect()
        {
            // Act
            _creatorFactory =
                new CreatorFactory(_creatorBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);

            // Arrange
            var creator = _creatorFactory.Create(_creatorId, _creatorName, _creatorRole);

            // Assert
            _creatorBusinessRulesCheckerMock.Verify(cbr => cbr.CreatorIdCanNotBeEmptyRuleIsBroken(_creatorId),
                Times.Once);
            _creatorBusinessRulesCheckerMock.Verify(cbr => cbr.CreatorNameCanNotBeEmptyRuleIsBroken(_creatorName),
                Times.Once);
            _creatorBusinessRulesCheckerMock.Verify(cbr => cbr.CreatorNameCanNotBeShorterThanRuleIsBroken(_creatorName),
                Times.Once);
            _creatorBusinessRulesCheckerMock.Verify(cbr => cbr.CreatorNameCanNotBeLongerThanRuleIsBroken(_creatorName),
                Times.Once);

            _domainEventEmitterMock.Verify(
                dve => dve.Emit(It.IsAny<Creator>(),
                    It.Is<NewCreatorCreatedDomainEvent>(de =>
                        de.CreatorId.Equals(creator.CreatorId) && de.CreatorName == creator.CreatorName &&
                        Equals(de.CreatorRole, creator.CreatorRole))), Times.Once);
        }
    }
}