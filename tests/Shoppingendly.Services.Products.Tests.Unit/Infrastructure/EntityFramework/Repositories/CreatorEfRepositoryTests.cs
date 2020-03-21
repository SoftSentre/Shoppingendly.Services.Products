using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moq;
using Microsoft.Extensions.Logging;
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
    public class CreatorEfRepositoryTests
    {
        private readonly Creator _creator = new Creator(new CreatorId(new Guid("FE2472FE-81C7-4C10-9D65-195CB820A33A")),
            "Creator", "creator@email.com", Role.Admin);

        [Fact]
        public async void CheckIfGetCreatorByIdAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);

            // Act
            var testResult = await creatorRepository.GetByIdAsync(_creator.Id);

            // Arrange
            testResult.Value.Name.Should().Be(_creator.Name);
            testResult.Value.Email.Should().Be(_creator.Email);
            testResult.Value.Role.Should().Be(_creator.Role);
            testResult.Value.CreatedAt.Should().Be(_creator.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfGetCreatorByNameAsyncMethodReturnValidObject()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);

            // Act
            var testResult = await creatorRepository.GetByNameAsync(_creator.Name);

            // Arrange
            testResult.Value.Name.Should().Be(_creator.Name);
            testResult.Value.Email.Should().Be(_creator.Email);
            testResult.Value.Role.Should().Be(_creator.Role);
            testResult.Value.CreatedAt.Should().Be(_creator.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfAddCreatorMethodSuccessfullyAddedItemToDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            await creatorRepository.AddAsync(creator);
            await dbContext.SaveChangesAsync();
            var testResult = dbContext.Creators.FirstOrDefault(p => p.Id.Equals(creator.Id));

            // Assert
            testResult.Name.Should().Be(creator.Name);
            testResult.Email.Should().Be(creator.Email);
            testResult.Role.Should().Be(creator.Role);
            testResult.CreatedAt.Should().Be(creator.CreatedAt);

            dbContext.Dispose();
        }

        [Fact]
        public async void CheckIfUpdateCreatorMethodChangedAExistingItemInDatabase()
        {
            // Arrange
            var dbContext = await CreateDbContext();
            ICreatorRepository creatorRepository = new CreatorEfRepository(dbContext);
            const string newCreatorEmail = "creator@email.com";

            // Act
            var creatorFromDatabase = await dbContext.Creators.FirstOrDefaultAsync(p => p.Id.Equals(_creator.Id));
            creatorFromDatabase.SetEmail(newCreatorEmail);
            creatorRepository.Update(creatorFromDatabase);
            await dbContext.SaveChangesAsync();

            var testResult = dbContext.Creators.FirstOrDefault(p => p.Id.Equals(_creator.Id));

            // Assert
            testResult.Email.Should().Be(newCreatorEmail);

            dbContext.Dispose();
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

            dbContext.Dispose();
        }

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
            await productServiceDbContext.SaveChangesAsync();

            return productServiceDbContext;
        }
    }
}