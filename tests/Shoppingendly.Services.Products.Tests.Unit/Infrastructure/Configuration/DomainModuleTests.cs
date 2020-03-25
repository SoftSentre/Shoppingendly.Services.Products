using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Infrastructure.Configuration.Domain;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration
{
    public class DomainModuleTests
    {
        [Fact]
        public void CheckIfAllTypesFromDomainModuleAreRegistered()
        {
            //Arrange
            var typesToCheck = new List<Type>
            {
                typeof(IProductDomainService),
                typeof(ICategoryDomainService),
                typeof(ICreatorDomainService)
            };

            var domainModule = new DomainModule();

            //Act
            var typesRegistered = domainModule.GetTypesRegisteredInModule().ToList();

            //Arrange
            typesRegistered.Should().Contain(typesToCheck);
        }
    }
}