using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;

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
                .UseSqlServer(Constants.ConnectionString)
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                .Options;

            var dbContext = new ProductServiceDbContext(options);
            var conventionSet = ConventionSet.CreateConventionSet(dbContext);
            var modelBuilder = new ModelBuilder(conventionSet);
            var entityTypeBuilder = modelBuilder.Entity<TEntity>();
            var entityConfiguration = entityTypeConfiguration;

            entityConfiguration.Configure(entityTypeBuilder);

            return entityTypeBuilder;
        }
    }
}