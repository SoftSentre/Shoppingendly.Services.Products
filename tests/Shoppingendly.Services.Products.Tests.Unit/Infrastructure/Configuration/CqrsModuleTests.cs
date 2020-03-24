using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Application.CQRS.Base.Bus;
using Shoppingendly.Services.Products.Infrastructure.Configuration.CQRS;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration
{
    public class CqrsModuleTests
    {
        [Fact]
        public void CheckIfAllTypesFromCqrsModuleAreRegistered()
        {
            //Arrange
            var typesToCheck = new List<Type>
            {
                typeof(ICommandBus),
                typeof(IQueryBus)
            };

            var cqrsModule = new CqrsModule();

            //Act
            var typesRegistered = cqrsModule.GetTypesRegisteredInModule().ToList();

            //Arrange
            typesRegistered.Should().Contain(typesToCheck);
        }
    }
}