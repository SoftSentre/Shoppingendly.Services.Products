using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Repositories
{
    public class ProductEfRepositoryTests
    {
        private readonly CreatorId _creatorId = new CreatorId(new Guid("12301ABE-24FE-41E5-A5F5-B6255C049CA1"));
        private readonly CategoryId _categoryId = new CategoryId(new Guid("1F9FEDB4-0F85-4E47-A4C3-F4C25F0E9996"));

        [Fact]
        public async void CheckIfGetProductByIdAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            var product = new Product(new ProductId(), _creatorId, "ExampleProductName", "ExampleProducer");
            
            // Act
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var testResult = await productRepository.GetByIdAsync(product.Id);
            
            // Arrange
            testResult.Value.Name.Should().Be(product.Name);
            testResult.Value.Producer.Should().Be(product.Producer);
            testResult.Value.CreatorId.Should().Be(product.CreatorId);
            testResult.Value.CreatedAt.Should().Be(product.CreatedAt);
            
            dbContext.Dispose();
        }
        
        [Fact]
        public async void CheckIfGetProductByNameAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            var product = new Product(new ProductId(), _creatorId, "ExampleProductName", "ExampleProducer");
            
            // Act
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var testResult = await productRepository.GetByNameAsync(product.Name);
            
            // Arrange
            testResult.Value.Name.Should().Be(product.Name);
            testResult.Value.Producer.Should().Be(product.Producer);
            testResult.Value.CreatorId.Should().Be(product.CreatorId);
            testResult.Value.CreatedAt.Should().Be(product.CreatedAt);
            
            dbContext.Dispose();
        }
        
        [Fact]
        public async void CheckIfGetProductByNameWithIncludesAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            
            var product = new Product(new ProductId(), _creatorId, "ExampleProductName", "ExampleProducer");
            product.AssignCategory(_categoryId);
            
            // Act
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var testResult = await productRepository.GetByNameWithIncludesAsync(product.Name);
            
            // Arrange
            testResult.Value.Name.Should().Be(product.Name);
            testResult.Value.Producer.Should().Be(product.Producer);
            testResult.Value.CreatorId.Should().Be(product.CreatorId);
            testResult.Value.CreatedAt.Should().Be(product.CreatedAt);
            testResult.Value.ProductCategories.FirstOrDefault()?.FirstKey.Should().Be(product.Id);
            testResult.Value.ProductCategories.FirstOrDefault()?.SecondKey.Should().Be(_categoryId);
            
            dbContext.Dispose();
        }
        
        [Fact]
        public async void CheckIfAddAsyncMethodSuccessfullyAddedItemToTheDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            var product = new Product(new ProductId(), _creatorId, "ExampleProductName", "ExampleProducer");

            // Act
            await productRepository.AddAsync(product);
            await dbContext.SaveChangesAsync();
            var testResult = dbContext.Products.FirstOrDefault(p => p.Id.Equals(product.Id));

            // Assert
            testResult?.Name.Should().Be(product.Name);
            testResult?.Producer.Should().Be(product.Producer);
            testResult?.CreatorId.Should().Be(product.CreatorId);
            testResult?.CreatedAt.Should().Be(product.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfAddMethodSuccessfullyAddedItemToDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            var product = new Product(new ProductId(), _creatorId, "ExampleProductName", "ExampleProducer");
            const string newProductName = "NewProductName";

            // Act
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            
            var productFromDatabase = await dbContext.Products.FirstOrDefaultAsync(p => p.Id.Equals(product.Id));
            productFromDatabase.SetName(newProductName);
            productRepository.Update(productFromDatabase);
            await dbContext.SaveChangesAsync();
            
            var testResult = dbContext.Products.FirstOrDefault(p => p.Id.Equals(product.Id));

            // Assert
            testResult?.Name.Should().Be(newProductName);
            
            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfDeleteMethodSuccessfullyRemovedItemFromDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            var product = new Product(new ProductId(), _creatorId, "ExampleProductName", "ExampleProducer");

            // Act
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            
            productRepository.Delete(product);
            await dbContext.SaveAsync();

            // Assert
            dbContext.Products.Should().BeEmpty();
            
            dbContext.Dispose();
        }

        private async Task<ProductServiceDbContext> CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ProductServiceDbContext>()
                .UseSqlServer(Constants.ConnectionString)
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                .Options;

            var productServiceDbContext = new ProductServiceDbContext(dbContextOptions);
            productServiceDbContext.Database.EnsureDeleted();
            productServiceDbContext.Database.EnsureCreated();
            await productServiceDbContext.Creators.AddAsync(
                new Creator(_creatorId, "Creator", "creator@email.com", Role.Admin));
            await productServiceDbContext.Categories.AddAsync(new Category(_categoryId, "ExampleCategory"));
            await productServiceDbContext.SaveAsync();

            return productServiceDbContext;
        }
    }
}