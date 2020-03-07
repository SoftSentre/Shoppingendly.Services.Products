using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Moq;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Extensions
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

            var loggerFactory = new Mock<ILoggerFactory>();
            var dbContext = new ProductServiceDbContext(new SqlSettings(), loggerFactory.Object, options);
            var conventionSet = ConventionSet.CreateConventionSet(dbContext);
            var modelBuilder = new ModelBuilder(conventionSet);
            var entityTypeBuilder = modelBuilder.Entity<TEntity>();
            var entityConfiguration = entityTypeConfiguration;

            entityConfiguration.Configure(entityTypeBuilder);

            return entityTypeBuilder;
        }
    }
}