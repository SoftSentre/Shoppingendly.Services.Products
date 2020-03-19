using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.Events.Categories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.DomainEvents
{
    public class DomainEventsDispatcherTests
    {
        [Fact]
        public async void CheckIfDispatchMethodMatchedAppropriateMethodOnceWhenAnyDomainEventRaised()
        {
            // Arrange
            var domainEventsAccessor = new Mock<IDomainEventAccessor>();
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
        public async void CheckIfDispatchMethodMatchedAppropriateMethodOnce()
        {
            // Arrange
            var domainEventsAccessor = new Mock<IDomainEventAccessor>();
            var domainEvents = new List<IDomainEvent>
            {
                new NewCategoryCreatedDomainEvent(new CategoryId(), "Name"),
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
        public async void CheckIfDispatchMethodThrowingExceptionWhenProvidedEventIsNull()
        {
            var domainEventsAccessor = new Mock<IDomainEventAccessor>();
            var domainEvents = new List<IDomainEvent> {null};

            domainEventsAccessor
                .Setup(dea => dea.GetUncommittedEvents()).Returns(
                    new Maybe<IEnumerable<IDomainEvent>>(domainEvents));

            domainEventsAccessor.Setup(dea => dea.DispatchEvents(domainEvents))
                .Throws<Exception>();

            var logger = new Mock<ILogger<DomainEventsDispatcher>>();
            var domainEventsDispatcher = new DomainEventsDispatcher(logger.Object, domainEventsAccessor.Object);

            // Act
            Func<Task> action = async () => await domainEventsDispatcher.DispatchAsync();

            // Assert
            action.Should().Throw<DispatchedDomainEventsFailedException>()
                .WithMessage("Error occured when dispatching the domain events. Message: Exception of type 'System.Exception' was thrown.");
        }
    }
}