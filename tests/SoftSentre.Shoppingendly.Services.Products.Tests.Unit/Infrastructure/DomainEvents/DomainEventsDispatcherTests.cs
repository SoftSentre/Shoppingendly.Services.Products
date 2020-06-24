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
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.DomainEvents
{
    public class DomainEventsDispatcherTests
    {
        [Fact]
        public async void CheckIfDispatchMethodMatchedAppropriateMethodOnce()
        {
            // Arrange
            var domainEventsAccessor = new Mock<IDomainEventAccessor>();
            var domainEvents = new List<IDomainEvent>
            {
                new NewCategoryCreatedDomainEvent(new CategoryId(), "Name", Picture.Empty),
                new CategoryDescriptionChangedDomainEvent(new CategoryId(), "Description")
            };

            domainEventsAccessor.Setup(dea => dea.GetUncommittedEvents()).Returns(
                new Maybe<IEnumerable<IDomainEvent>>(domainEvents));

            var logger = new Mock<ILogger<DomainEventsDispatcher>>();
            var domainEventsDispatcher = new DomainEventsDispatcher(logger.Object, domainEventsAccessor.Object);

            // Act
            await domainEventsDispatcher.DispatchAsync();

            // Assert
            domainEventsAccessor.Verify(dea => dea.GetUncommittedEvents(), Times.Once);
            domainEventsAccessor.Verify(dea => dea.DispatchEvents(domainEvents), Times.Once);
            domainEventsAccessor.Verify(dea => dea.ClearAllDomainEvents(), Times.Once);
        }

        [Fact]
        public async void CheckIfDispatchMethodMatchedAppropriateMethodOnceWhenAnyDomainEventRaised()
        {
            // Arrange
            var domainEventsAccessor = new Mock<IDomainEventAccessor>();
            domainEventsAccessor.Setup(dea => dea.GetUncommittedEvents())
                .Returns(new Maybe<IEnumerable<IDomainEvent>>());

            var logger = new Mock<ILogger<DomainEventsDispatcher>>();
            var domainEventsDispatcher = new DomainEventsDispatcher(logger.Object, domainEventsAccessor.Object);

            // Act
            await domainEventsDispatcher.DispatchAsync();

            // Assert
            domainEventsAccessor.Verify(dea => dea.GetUncommittedEvents(), Times.Once);
            domainEventsAccessor.Verify(dea => dea.DispatchEvents(new List<IDomainEvent>()), Times.Never);
            domainEventsAccessor.Verify(dea => dea.ClearAllDomainEvents(), Times.Never);
        }

        [Fact]
        public void CheckIfDispatchMethodThrowingExceptionWhenProvidedEventIsNull()
        {
            // Arrange
            var domainEventsAccessor = new Mock<IDomainEventAccessor>();
            var domainEvents = new List<IDomainEvent> {null};

            domainEventsAccessor
                .Setup(dea => dea.GetUncommittedEvents()).Returns(
                    new Maybe<IEnumerable<IDomainEvent>>(domainEvents));

            domainEventsAccessor.Setup(dea => dea.DispatchEvents(domainEvents))
                .Throws<DomainEventCanNotBeEmptyException>();

            var logger = new Mock<ILogger<DomainEventsDispatcher>>();
            var domainEventsDispatcher = new DomainEventsDispatcher(logger.Object, domainEventsAccessor.Object);

            // Act
            Func<Task> action = async () => await domainEventsDispatcher.DispatchAsync();

            // Assert
            action.Should().Throw<DispatchedDomainEventsFailedException>()
                .WithInnerExceptionExactly<DomainEventCanNotBeEmptyException>();
        }
    }
}