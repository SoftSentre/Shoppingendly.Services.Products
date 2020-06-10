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
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.Repositories
{
    public class CategoryEfRepositoryTests
    {
        private readonly Category _category =
            new Category(new CategoryId(new Guid("1F9FEDB4-0F85-4E47-A4C3-F4C25F0E9996")), "ExampleCategory",
                "ExampleCategoryDescription");

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

            await productServiceDbContext.Database.EnsureDeletedAsync();
            await productServiceDbContext.Database.EnsureCreatedAsync();
            await productServiceDbContext.Categories.AddAsync(_category);
            await productServiceDbContext.Products.AddRangeAsync(new Product(
                    new ProductId(new Guid("BD31DDB6-CEA1-493C-B49E-BFC902EF1F14")),
                    new CreatorId(new Guid("12301ABE-24FE-41E5-A5F5-B6255C049CA1")),
                    "ExampleProductName", ProductProducer.CreateProductProducer("ExampleProducer")),
                new Product(new ProductId(new Guid("C3241FC4-AD0F-40AE-A8B2-D8F848DD1D17")),
                    new CreatorId(new Guid("12301ABE-24FE-41E5-A5F5-B6255C049CA1")),
                    "ExampleSomeProductName", ProductProducer.CreateProductProducer("ExampleSomeProducer")));
            await productServiceDbContext.ProductCategories.AddRangeAsync(
                new ProductCategory(new ProductId(new Guid("BD31DDB6-CEA1-493C-B49E-BFC902EF1F14")), _category.CategoryId),
                new ProductCategory(new ProductId(new Guid("C3241FC4-AD0F-40AE-A8B2-D8F848DD1D17")), _category.CategoryId));
            await productServiceDbContext.SaveChangesAsync();

            return productServiceDbContext;
        }

        [Fact]
        public async void CheckIdGetCategoryMethodWithIncludesAsyncReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);

            // Act
            var testResult = await categoryRepository.GetByNameWithIncludesAsync(_category.CategoryName);

            // Assert
            testResult.Value.CategoryName.Should().Be(_category.CategoryName);
            testResult.Value.CategoryDescription.Should().Be(_category.CategoryDescription);
            testResult.Value.CreatedAt.Should().Be(_category.CreatedAt);
            testResult.Value.ProductCategories.Should().HaveCount(2);

            await dbContext.DisposeAsync();
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
            var testResult = dbContext.Categories.FirstOrDefault(p => p.CategoryId.Equals(category.CategoryId)) ?? It.IsAny<Category>();

            // Assert
            testResult.CategoryName.Should().Be(category.CategoryName);
            testResult.CategoryDescription.Should().Be(category.CategoryDescription);
            testResult.CreatedAt.Should().Be(category.CreatedAt);

            await dbContext.DisposeAsync();
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

            await dbContext.DisposeAsync();
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

            await dbContext.DisposeAsync();
        }

        [Fact]
        public async void CheckIfGetAllCategoriesWithIncludesMethodReturnValidObjects()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);
            await dbContext.AddRangeAsync(
                new Category(new CategoryId(), "Name"),
                new Category(new CategoryId(), "OtherName"));
            await dbContext.SaveChangesAsync();

            // Act
            var testResult = await categoryRepository.GetAllWithIncludesAsync();

            // Assert
            testResult.Value.Should().HaveCount(3);
            var testResultItem = testResult.Value.FirstOrDefault(c => c.CategoryName == _category.CategoryName) ??
                                 It.IsAny<Category>();
            testResultItem.ProductCategories.Should().HaveCount(2);

            await dbContext.DisposeAsync();
        }

        [Fact]
        public async void CheckIfGetCategoryByIdAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);

            // Act
            var testResult = await categoryRepository.GetByIdAsync(_category.CategoryId);

            // Assert
            testResult.Value.CategoryName.Should().Be(_category.CategoryName);
            testResult.Value.CategoryDescription.Should().Be(_category.CategoryDescription);
            testResult.Value.CreatedAt.Should().Be(_category.CreatedAt);

            await dbContext.DisposeAsync();
        }

        [Fact]
        public async void CheckIfGetCategoryByNameAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);

            // Act
            var testResult = await categoryRepository.GetByNameAsync(_category.CategoryName);

            // Assert
            testResult.Value.CategoryName.Should().Be(_category.CategoryName);
            testResult.Value.CategoryDescription.Should().Be(_category.CategoryDescription);
            testResult.Value.CreatedAt.Should().Be(_category.CreatedAt);

            await dbContext.DisposeAsync();
        }

        [Fact]
        public async void CheckIfUpdateCategoryMethodChangedAExistingItemInDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICategoryRepository categoryRepository = new CategoryEfRepository(dbContext);
            const string newCategoryName = "otherCategoryName";

            // Act
            var categoryFromDatabase = await dbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId.Equals(_category.CategoryId));
            categoryFromDatabase.SetCategoryName(newCategoryName);
            categoryRepository.Update(categoryFromDatabase);
            await dbContext.SaveChangesAsync();

            var testResult = dbContext.Categories.FirstOrDefault(p => p.CategoryId.Equals(_category.CategoryId)) ??
                             It.IsAny<Category>();

            // Assert
            testResult.CategoryName.Should().Be(newCategoryName);

            await dbContext.DisposeAsync();
        }
    }
}