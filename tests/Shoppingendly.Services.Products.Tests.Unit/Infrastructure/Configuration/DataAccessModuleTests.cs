using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shoppingendly.Services.Products.Application.Mapper;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Infrastructure.Configuration.Data;
using Shoppingendly.Services.Products.Infrastructure.Configuration.Mappings;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;
using Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration
{
    public class DataAccessModuleTests
    {
        [Fact]
        public void CheckIfAllTypesFromDataAccessModuleAreRegistered()
        {
            //Arrange
            var loggerFactory = new Mock<ILoggerFactory>();
            var typesToCheck = new List<Type>
            {
                typeof(ICategoryRepository),
                typeof(ICreatorRepository),
                typeof(IProductRepository),
                typeof(IUnitOfWork),
                typeof(ProductServiceDbContext),
                typeof(SqlSettings),
            };

            var dataAccessModule = new DataAccessModule(loggerFactory.Object);

            //Act
            var typesRegistered = dataAccessModule.GetTypesRegisteredInModule().ToList();

            //Arrange
            typesRegistered.Should().Contain(typesToCheck);
        }
    }
}