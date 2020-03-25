using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Shoppingendly.Services.Products.Application.Mapper;
using Shoppingendly.Services.Products.Infrastructure.Configuration.Mappings;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration
{
    public class MappingModuleTests
    {
        [Fact]
        public void CheckIfAllTypesFromMappingModuleAreRegistered()
        {
            //Arrange
            var typesToCheck = new List<Type>
            {
                typeof(IMapperWrapper),
                typeof(IMapper),
                typeof(MapperConfiguration)
            };

            var mappingModule = new MappingModule();

            //Act
            var typesRegistered = mappingModule.GetTypesRegisteredInModule().ToList();

            //Arrange
            typesRegistered.Should().Contain(typesToCheck);
        }
    }
}