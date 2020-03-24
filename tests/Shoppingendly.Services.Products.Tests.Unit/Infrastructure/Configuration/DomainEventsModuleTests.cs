using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Infrastructure.Configuration.DomainEvents;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Decorators;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration
{
    public class DomainEventsModuleTests
    {
        [Fact]
        public void CheckIfAllTypesFromDomainEventsModuleAreRegistered()
        {
            //Arrange
            var typesToCheck = new List<Type>
            {
                typeof(IDomainEventAccessor),
                typeof(IDomainEventPublisher),
                typeof(IDomainEventsDispatcher)
            };

            var cqrsModule = new DomainEventsModule();

            //Act
            var typesRegistered = cqrsModule.GetTypesRegisteredInModule().ToList();

            //Arrange
            typesRegistered.Should().Contain(typesToCheck);
        }
    }
}