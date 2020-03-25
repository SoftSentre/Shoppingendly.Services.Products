using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Infrastructure.Configuration.Logging;
using Shoppingendly.Services.Products.Infrastructure.Logger;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration
{
    public class LoggingModuleTests
    {
        [Fact]
        public void CheckIfAllTypesFromLoggingModuleAreRegistered()
        {
            //Arrange
            var typesToCheck = new List<Type>
            {
                typeof(LoggerSettings)
            };

            var cqrsModule = new LoggingModule();

            //Act
            var typesRegistered = cqrsModule.GetTypesRegisteredInModule().ToList();

            //Arrange
            typesRegistered.Should().Contain(typesToCheck);
        }
    }
}