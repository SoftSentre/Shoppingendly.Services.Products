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
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Data;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;
using SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration
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