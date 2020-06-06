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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.EntityFramework.DomainEvents
{
    public class DomainEventEfAccessorTests
    {
        private static async Task<ProductServiceDbContext> CreateDbContext()
        {
            var dbName = Guid.NewGuid().ToString();

            var dbContextOptions = new DbContextOptionsBuilder<ProductServiceDbContext>()
                .UseInMemoryDatabase(dbName)
                .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                .Options;

            var loggerFactory = new Mock<ILoggerFactory>();
            var domainEventDispatcher = new Mock<IDomainEventsDispatcher>().Object;
            var productServiceDbContext =
                new ProductServiceDbContext(loggerFactory.Object, domainEventDispatcher, new SqlSettings(),
                    dbContextOptions);
            productServiceDbContext.Database.EnsureDeleted();
            productServiceDbContext.Database.EnsureCreated();

            var creator = new Creator(new CreatorId(new Guid("FE2472FE-81C7-4C10-9D65-195CB820A33A")), "Creator",
                Role.Admin);
            await productServiceDbContext.Creators.AddAsync(creator);
            await productServiceDbContext.SaveChangesAsync();

            return productServiceDbContext;
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
            var testEventResult = (NewCreatorCreatedDomainEvent) testEventsResult.Value.FirstOrDefault() ??
                                  It.IsAny<NewCreatorCreatedDomainEvent>();

            testEventResult.Should().NotBeNull();
            testEventResult.Should().BeOfType<NewCreatorCreatedDomainEvent>();
            testEventResult.CreatorId.Id.Should().Be(new Guid("FE2472FE-81C7-4C10-9D65-195CB820A33A"));
            testEventResult.Name.Should().Be("Creator");
            testEventResult.Role.Should().Be(Role.Admin);
        }

        [Fact]
        public async void CheckIfThePublishMethodFromDomainEventPublisherWillBeMatchedOnce()
        {
            // Arrange
            var domainEventPublisher = new Mock<IDomainEventPublisher>();
            var categoryCreatedEvent = new NewCategoryCreatedDomainEvent(new CategoryId(), "Name");
            var creatorCreatedEvent =
                new NewCreatorCreatedDomainEvent(new CreatorId(), "Name", Role.Moderator);
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
    }
}