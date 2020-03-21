using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moq;
using Microsoft.Extensions.Logging;
using Moq;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Repositories
{
    public class CategoryEfRepositoryTests
    {
        private readonly Category _category =
            new Category(new CategoryId(new Guid("1F9FEDB4-0F85-4E47-A4C3-F4C25F0E9996")), "ExampleCategory",
                "ExampleCategoryDescription");

        [Fact]
        public async void CheckIfGetCategoryByIdAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);

            // Act
            var testResult = await categoryRepository.GetByIdAsync(_category.Id);

            // Assert
            testResult.Value.Name.Should().Be(_category.Name);
            testResult.Value.Description.Should().Be(_category.Description);
            testResult.Value.CreatedAt.Should().Be(_category.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfGetCategoryByNameAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);

            // Act
            var testResult = await categoryRepository.GetByNameAsync(_category.Name);

            // Assert
            testResult.Value.Name.Should().Be(_category.Name);
            testResult.Value.Description.Should().Be(_category.Description);
            testResult.Value.CreatedAt.Should().Be(_category.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIdGetCategoryMethodWithIncludesAsyncReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);

            // Act
            var testResult = await categoryRepository.GetByNameWithIncludesAsync(_category.Name);

            // Assert
            testResult.Value.Name.Should().Be(_category.Name);
            testResult.Value.Description.Should().Be(_category.Description);
            testResult.Value.CreatedAt.Should().Be(_category.CreatedAt);
            testResult.Value.ProductCategories.Should().HaveCount(2);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfGetAllCategoriesMethodReturnValidObjects()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);
            dbContext.AddRange(
                new Category(new CategoryId(), "Name"),
                new Category(new CategoryId(), "OtherName"));
            await dbContext.SaveChangesAsync();

            // Act
            var testResult = await categoryRepository.GetAllAsync();

            // Assert
            testResult.Value.Should().HaveCount(3);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfGetAllCategoriesWithIncludesMethodReturnValidObjects()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);
            dbContext.AddRange(
                new Category(new CategoryId(), "Name"),
                new Category(new CategoryId(), "OtherName"));
            await dbContext.SaveChangesAsync();

            // Act
            var testResult = await categoryRepository.GetAllWithIncludesAsync();

            // Assert
            testResult.Value.Should().HaveCount(3);
            testResult.Value.FirstOrDefault(c => c.Name == _category.Name)
                .ProductCategories.Should().HaveCount(2);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfAddCategoryMethodSuccessfullyAddedItemToDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);
            var category = new Category(new CategoryId(), "OtherExampleCategory", "OtherExampleCategoryDescription");

            // Act
            await categoryRepository.AddAsync(category);
            await dbContext.SaveChangesAsync();
            var testResult = dbContext.Categories.FirstOrDefault(p => p.Id.Equals(category.Id));

            // Assert
            testResult.Name.Should().Be(category.Name);
            testResult.Description.Should().Be(category.Description);
            testResult.CreatedAt.Should().Be(category.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfUpdateCategoryMethodChangedAExistingItemInDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);
            const string newCategoryName = "otherCategoryName";

            // Act
            var categoryFromDatabase = await dbContext.Categories.FirstOrDefaultAsync(p => p.Id.Equals(_category.Id));
            categoryFromDatabase.SetName(newCategoryName);
            categoryRepository.Update(categoryFromDatabase);
            await dbContext.SaveChangesAsync();

            var testResult = dbContext.Categories.FirstOrDefault(p => p.Id.Equals(_category.Id));

            // Assert
            testResult.Name.Should().Be(newCategoryName);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfDeleteCategoryMethodSuccessfullyRemovedItemFromDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);

            // Act
            categoryRepository.Delete(_category);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Creators.Should().BeEmpty();

            dbContext.Dispose();
        }

        private async Task<ProductServiceDbContext> CreateDbContext()
        {
            var dbName = Guid.NewGuid().ToString();

            var loggerFactory = new Mock<ILoggerFactory>();
            var dbContextOptions = new DbContextOptionsBuilder<ProductServiceDbContext>()
                .UseInMemoryDatabase(dbName)
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                .Options;

            var domainEventDispatcher = new Mock<IDomainEventsDispatcher>().Object;
            var productServiceDbContext =
                new ProductServiceDbContext(new SqlSettings(), loggerFactory.Object, domainEventDispatcher, dbContextOptions);
            productServiceDbContext.Database.EnsureDeleted();
            productServiceDbContext.Database.EnsureCreated();

            await productServiceDbContext.Categories.AddAsync(_category);
            await productServiceDbContext.Products.AddRangeAsync(new Product(
                    new ProductId(new Guid("BD31DDB6-CEA1-493C-B49E-BFC902EF1F14")),
                    new CreatorId(new Guid("12301ABE-24FE-41E5-A5F5-B6255C049CA1")),
                    "ExampleProductName", "ExampleProducer"),
                new Product(new ProductId(new Guid("C3241FC4-AD0F-40AE-A8B2-D8F848DD1D17")),
                    new CreatorId(new Guid("12301ABE-24FE-41E5-A5F5-B6255C049CA1")),
                    "ExampleSomeProductName", "ExampleSomeProducer"));
            await productServiceDbContext.ProductCategories.AddRangeAsync(
                new ProductCategory(new ProductId(new Guid("BD31DDB6-CEA1-493C-B49E-BFC902EF1F14")), _category.Id),
                new ProductCategory(new ProductId(new Guid("C3241FC4-AD0F-40AE-A8B2-D8F848DD1D17")), _category.Id));
            await productServiceDbContext.SaveChangesAsync();

            return productServiceDbContext;
        }
    }
}