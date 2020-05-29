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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions
{
    public static class ConfigurationMetadataTestsExtensions
    {
        public static EntityTypeBuilder<TEntity> GetCustomerEntityConfigurationMetadata<TEntity, TEntityConfiguration>(
            TEntityConfiguration entityTypeConfiguration)
            where TEntity : class
            where TEntityConfiguration : class, IEntityTypeConfiguration<TEntity>
        {
            var options = new DbContextOptionsBuilder<ProductServiceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                .Options;

            var domainEventDispatcher = new Mock<IDomainEventsDispatcher>().Object;
            var loggerFactory = new Mock<ILoggerFactory>();
            var dbContext = new ProductServiceDbContext(loggerFactory.Object, domainEventDispatcher, 
                new SqlSettings(), options);
            var conventionSet = ConventionSet.CreateConventionSet(dbContext);
            var modelBuilder = new ModelBuilder(conventionSet);
            var entityTypeBuilder = modelBuilder.Entity<TEntity>();
            var entityConfiguration = entityTypeConfiguration;

            entityConfiguration.Configure(entityTypeBuilder);

            return entityTypeBuilder;
        }
    }
}