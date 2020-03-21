using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moq;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Categories;
using Shoppingendly.Services.Products.Core.Domain.Events.Creators;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.DomainEvents;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.DomainEvents
{
    public class DomainEventEfAccessorTests
    {
        [Fact]
        public async void CheckIfGetEntriesMethodReturnDomainEvents()
        {
            // Arrange
            var domainEventPublisher = new Mock<IDomainEventPublisher>();
            var domainEventsAccessor = new DomainEventsEfAccessor(domainEventPublisher.Object, await CreateDbContext());

            // Act
            var testEventsResult = domainEventsAccessor.GetUncommittedEvents();

            // Assert
            testEventsResult.Value.Should().NotBeNull();
            var testEventResult = (NewCreatorCreatedDomainEvent) testEventsResult.Value.FirstOrDefault();
            testEventResult.Should().NotBeNull();
            testEventResult.Should().BeOfType<NewCreatorCreatedDomainEvent>();
            testEventResult.CreatorId.Id.Should().Be(new Guid("FE2472FE-81C7-4C10-9D65-195CB820A33A"));
            testEventResult.Name.Should().Be("Creator");
            testEventResult.Email.Should().Be("creator@email.com");
            testEventResult.Role.Should().Be(Role.Admin);
        }

        [Fact]
        public async void CheckIfThePublishMethodFromDomainEventPublisherWillBeMatchedOnce()
        {
            // Arrange
            var domainEventPublisher = new Mock<IDomainEventPublisher>();
            var categoryCreatedEvent = new NewCategoryCreatedDomainEvent(new CategoryId(), "Name");
            var creatorCreatedEvent = new NewCreatorCreatedDomainEvent(new CreatorId(), "Name", "email@email.com", Role.Moderator);
            var domainEventsAccessor = new DomainEventsEfAccessor(domainEventPublisher.Object, await CreateDbContext());
            
            var domainEvents = new List<IDomainEvent>
            {
                categoryCreatedEvent,
                creatorCreatedEvent
            };
            
            // Act
            domainEventsAccessor.DispatchEvents(domainEvents);

            // Assert
            domainEventPublisher.Verify(dep => dep.PublishAsync<IDomainEvent>(categoryCreatedEvent), Times.Once);
            domainEventPublisher.Verify(dep => dep.PublishAsync<IDomainEvent>(creatorCreatedEvent), Times.Once);
        }
        
        [Fact]
        public async void CheckIfDomainEventsListWillBeEmptyAfterClearDomainEventsMethodWillBeFired()
        {
            // Arrange
            var domainEventPublisher = new Mock<IDomainEventPublisher>();
            var domainEventsAccessor = new DomainEventsEfAccessor(domainEventPublisher.Object, await CreateDbContext());
            
            // Act
            domainEventsAccessor.ClearAllDomainEvents();

            // Assert
            var testResult = domainEventsAccessor.GetUncommittedEvents();
            testResult.Value.Should().BeEmpty();
        }
        
        private async Task<ProductServiceDbContext> CreateDbContext()
        {
            var dbName = Guid.NewGuid().ToString();

            var dbContextOptions = new DbContextOptionsBuilder<ProductServiceDbContext>()
                .UseInMemoryDatabase(dbName)
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                .Options;

            var domainEventDispatcher = new Mock<IDomainEventsDispatcher>().Object;
            var productServiceDbContext = new ProductServiceDbContext(dbContextOptions, domainEventDispatcher);
            productServiceDbContext.Database.EnsureDeleted();
            productServiceDbContext.Database.EnsureCreated();

            var creator = new Creator(new CreatorId(new Guid("FE2472FE-81C7-4C10-9D65-195CB820A33A")),
                "Creator", "creator@email.com", Role.Admin);
            await productServiceDbContext.Creators.AddAsync(creator);
            await productServiceDbContext.SaveChangesAsync();

            return productServiceDbContext;
        }    
    }
}