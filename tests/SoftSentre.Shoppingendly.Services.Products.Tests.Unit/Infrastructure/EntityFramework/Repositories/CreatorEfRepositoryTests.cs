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
    public class CreatorEfRepositoryTests
    {
        private readonly Creator _creator = new Creator(new CreatorId(new Guid("FE2472FE-81C7-4C10-9D65-195CB820A33A")),
            "Creator", CreatorRole.Admin);

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
            await productServiceDbContext.Creators.AddAsync(_creator);
            await productServiceDbContext.SaveChangesAsync();

            return productServiceDbContext;
        }

        [Fact]
        public async void CheckIfAddCreatorMethodSuccessfullyAddedItemToDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            await creatorRepository.AddAsync(creator);
            await dbContext.SaveChangesAsync();
            var testResult = dbContext.Creators.FirstOrDefault(p => p.Id.Equals(creator.Id)) ?? It.IsAny<Creator>();

            // Assert
            testResult.CreatorName.Should().Be(creator.CreatorName);
            testResult.CreatorRole.Should().Be(creator.CreatorRole);
            testResult.CreatedAt.Should().Be(creator.CreatedAt);

            await dbContext.DisposeAsync();
        }

        [Fact]
        public async void CheckIfDeleteCreatorMethodSuccessfullyRemovedItemFromDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);

            // Act
            creatorRepository.Delete(_creator);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Creators.Should().BeEmpty();

            await dbContext.DisposeAsync();
        }

        [Fact]
        public async void CheckIfGetCreatorByIdAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);

            // Act
            var testResult = await creatorRepository.GetByIdAsync(_creator.Id);

            // Arrange
            testResult.Value.CreatorName.Should().Be(_creator.CreatorName);
            testResult.Value.CreatorRole.Should().Be(_creator.CreatorRole);
            testResult.Value.CreatedAt.Should().Be(_creator.CreatedAt);

            await dbContext.DisposeAsync();
        }

        [Fact]
        public async void CheckIfGetCreatorByNameAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);

            // Act
            var testResult = await creatorRepository.GetByNameAsync(_creator.CreatorName);

            // Arrange
            testResult.Value.CreatorName.Should().Be(_creator.CreatorName);
            testResult.Value.CreatorRole.Should().Be(_creator.CreatorRole);
            testResult.Value.CreatedAt.Should().Be(_creator.CreatedAt);

            await dbContext.DisposeAsync();
        }

        [Fact]
        public async void CheckIfUpdateCreatorMethodChangedAExistingItemInDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);
            const string name = "creatorName";

            // Act
            var creatorFromDatabase = await dbContext.Creators.FirstOrDefaultAsync(p => p.Id.Equals(_creator.Id));
            creatorFromDatabase.SetCreatorName(name);
            creatorRepository.Update(creatorFromDatabase);
            await dbContext.SaveChangesAsync();

            var testResult = dbContext.Creators.FirstOrDefault(p => p.Id.Equals(_creator.Id)) ?? It.IsAny<Creator>();

            // Assert
            testResult.CreatorName.Should().Be(name);

            await dbContext.DisposeAsync();
        }
    }
}