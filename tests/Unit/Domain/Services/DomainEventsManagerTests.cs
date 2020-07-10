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
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Services 
{
    public class DomainEventsManagerTests : IAsyncLifetime
    {
        private IDomainEventsManager _domainEventsManager;
        private NewCreatorCreatedDomainEvent _newCreatorCreatedDomainEvent;
        private CreatorId _creatorId;
        private string _creatorName;
        private CreatorRole _creatorRole;
        private Creator _creator;

        public async Task InitializeAsync()
        {
            _domainEventsManager = new DomainEventsManager();
            _creatorId = new CreatorId(new Guid("7D108A30-C6F1-440F-8BB0-9DD1EA3D0258"));
            _creatorName = "exampleCreatorName";
            _creatorRole = CreatorRole.User;
            _creator = new Creator(_creatorId, _creatorName, _creatorRole);
            _newCreatorCreatedDomainEvent = new NewCreatorCreatedDomainEvent(_creatorId, _creatorName, _creatorRole);

            await Task.CompletedTask;
        }

        [Fact]
        public void GetUncommittedShouldSuccessfullyReturnDomainEvents()
        {
            // Arrange 
            _creator.DomainEvents.Add(_newCreatorCreatedDomainEvent);

            // Act
            var domainEvents = _domainEventsManager.GetUncommittedDomainEvents(_creator);

            // Assert
            domainEvents.Should().NotBeEmpty();
            _creator.DomainEvents.FirstOrDefault().Should().Be(_newCreatorCreatedDomainEvent);
        }

        [Fact]
        public void GetUncommitedShouldBeFailedWhenCreatorIsNull()
        {
            // Arrange 
            _creator = null;

            // Act
            Action getUncommitted = () => _domainEventsManager.GetUncommittedDomainEvents(_creator);

            // Assert
            getUncommitted.Should().Throw<GetUncommittedDomainEventsFailedException>()
                .Where(e => e.Code == ErrorCodes.GetUncommittedDomainEventsFailed)
                .WithMessage(
                    "Unable to get uncommitted domain events, because entity is null or domain events list are not initialized.");
        }

        [Fact]
        public void ClearDomainEventsShouldSuccessfullyRemovedAllDomainEvents()
        {
            // Arrange 
            _creator.DomainEvents.Add(_newCreatorCreatedDomainEvent);

            // Act
            _domainEventsManager.ClearAllDomainEvents(_creator);

            // Assert
            _creator.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void ClearDomainEventsShouldBeFailedWhenCreatorIsNull()
        {
            // Arrange 
            _creator = null;

            // Act
            Action getUncommitted = () => _domainEventsManager.ClearAllDomainEvents(_creator);

            // Assert
            getUncommitted.Should().Throw<ClearDomainEventsFailedException>()
                .Where(e => e.Code == ErrorCodes.ClearDomainEventsFailed)
                .WithMessage("Unable to clear domain events, because entity is null.");
        }

        public async Task DisposeAsync()
        {
            _domainEventsManager = null;
            _creatorId = null;
            _creatorName = null;
            _creatorRole = null;
            _creator = null;
            _newCreatorCreatedDomainEvent = null;

            await Task.CompletedTask;
        }
    }
}