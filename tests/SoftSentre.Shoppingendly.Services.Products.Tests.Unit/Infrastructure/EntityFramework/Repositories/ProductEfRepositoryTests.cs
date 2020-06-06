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
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Repositories
{
    public class ProductEfRepositoryTests
    {
        private readonly Creator _creator = new Creator(new CreatorId(new Guid("12301ABE-24FE-41E5-A5F5-B6255C049CA1")),
            "Creator", Role.Admin);

        private readonly Category _category =
            new Category(new CategoryId(new Guid("1F9FEDB4-0F85-4E47-A4C3-F4C25F0E9996")), "ExampleCategory");

        private readonly Product _product = new Product(new ProductId(),
            new CreatorId(new Guid("12301ABE-24FE-41E5-A5F5-B6255C049CA1")), "ExampleProductName", "ExampleProducer");

        private async Task<ProductServiceDbContext> CreateDbContext()
        {
            var dbName = Guid.NewGuid().ToString();

            var dbContextOptions = new DbContextOptionsBuilder<ProductServiceDbContext>()
                .UseInMemoryDatabase(dbName)
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                .Options;

            var loggerFactory = new Mock<ILoggerFactory>();
            var domainEventDispatcher = new Mock<IDomainEventsDispatcher>().Object;
            var productServiceDbContext = new ProductServiceDbContext(loggerFactory.Object, domainEventDispatcher,
                new SqlSettings(), dbContextOptions);
            productServiceDbContext.Database.EnsureDeleted();
            productServiceDbContext.Database.EnsureCreated();

            await productServiceDbContext.Creators.AddAsync(_creator);
            await productServiceDbContext.Categories.AddAsync(_category);
            await productServiceDbContext.Products.AddAsync(_product);
            await productServiceDbContext.SaveAsync();

            return productServiceDbContext;
        }

        [Fact]
        public async void CheckIfAddAsyncMethodSuccessfullyAddedItemToTheDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            var product = new Product(new ProductId(), _creator.Id, "ExampleProductName", "ExampleProducer");
            await productRepository.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Act
            var testResult = dbContext.Products.FirstOrDefault(p => p.Id.Equals(product.Id)) ?? It.IsAny<Product>();

            // Assert
            testResult.Name.Should().Be(product.Name);
            testResult.Producer.Should().Be(product.Producer);
            testResult.CreatorId.Should().Be(product.CreatorId);
            testResult.CreatedAt.Should().Be(product.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfAddMethodSuccessfullyAddedItemToDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            const string newProductName = "NewProductName";

            // Act
            var productFromDatabase = await dbContext.Products.FirstOrDefaultAsync(p => p.Id.Equals(_product.Id));
            productFromDatabase.SetName(newProductName);
            productRepository.Update(productFromDatabase);
            await dbContext.SaveChangesAsync();

            var testResult = dbContext.Products.FirstOrDefault(p => p.Id.Equals(_product.Id)) ?? It.IsAny<Product>();

            // Assert
            testResult.Name.Should().Be(newProductName);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfDeleteMethodSuccessfullyRemovedItemFromDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);

            // Act
            productRepository.Delete(_product);
            await dbContext.SaveAsync();

            // Assert
            dbContext.Products.Should().BeEmpty();

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfGetManyProductByNameAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);

            // Act
            var testResult = await productRepository.GetManyByNameAsync(_product.Name);

            // Assert
            testResult.Value.Should().HaveCount(1);
            var firstItem = testResult.Value.FirstOrDefault() ?? It.IsAny<Product>();
            firstItem.Name.Should().Be(_product.Name);
            firstItem.Producer.Should().Be(_product.Producer);
            firstItem.CreatorId.Should().Be(_product.CreatorId);
            firstItem.CreatedAt.Should().Be(_product.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfGetProductByIdAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);

            // Act
            var testResult = await productRepository.GetByIdAsync(_product.Id);

            // Assert
            testResult.Value.Name.Should().Be(_product.Name);
            testResult.Value.Producer.Should().Be(_product.Producer);
            testResult.Value.CreatorId.Should().Be(_product.CreatorId);
            testResult.Value.CreatedAt.Should().Be(_product.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfGetProductByIdWithIncludesAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            var product = new Product(new ProductId(), _creator.Id, "OtherProductName", "ExampleProducer");
            product.AssignCategory(_category.Id);
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Act
            var testResult = await productRepository.GetByIdWithIncludesAsync(product.Id);

            // Assert
            testResult.Value.Name.Should().Be(product.Name);
            testResult.Value.Producer.Should().Be(product.Producer);
            testResult.Value.CreatorId.Should().Be(product.CreatorId);
            testResult.Value.CreatedAt.Should().Be(product.CreatedAt);

            var firstChild = testResult.Value.ProductCategories.FirstOrDefault() ?? It.IsAny<ProductCategory>();
            firstChild.FirstKey.Should().Be(product.Id);
            firstChild.SecondKey.Should().Be(_category.Id);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfGetProductByNameWithIncludesAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            IProductRepository productRepository = new ProductEfRepository(dbContext);
            var product = new Product(new ProductId(), _creator.Id, "OtherProductName", "ExampleProducer");
            product.AssignCategory(_category.Id);
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Act
            var testResult = await productRepository.GetManyByNameWithIncludesAsync(product.Name);

            // Assert
            testResult.Value.Should().HaveCount(1);

            var firstItem = testResult.Value.FirstOrDefault() ?? It.IsAny<Product>();
            firstItem.Name.Should().Be(product.Name);
            firstItem.Producer.Should().Be(product.Producer);
            firstItem.CreatorId.Should().Be(product.CreatorId);
            firstItem.CreatedAt.Should().Be(product.CreatedAt);

            var firstChild = firstItem.ProductCategories.FirstOrDefault() ?? It.IsAny<ProductCategory>();
            firstChild.FirstKey.Should().Be(product.Id);
            firstChild.SecondKey.Should().Be(_category.Id);

            dbContext.Dispose();
        }
    }
}