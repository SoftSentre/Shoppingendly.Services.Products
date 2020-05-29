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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using SoftSentre.Shoppingendly.Services.Products.Core.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Core.Types;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.DomainEvents
{
    public class DomainEventsEfAccessor : IDomainEventAccessor
    {
        private readonly IDomainEventPublisher _domainEventPublisher;
        private readonly ProductServiceDbContext _productServiceDbContext;

        public DomainEventsEfAccessor(
            IDomainEventPublisher domainEventPublisher,
            ProductServiceDbContext productServiceDbContext)
        {
            _domainEventPublisher = domainEventPublisher.IfEmptyThenThrowAndReturnValue();
            _productServiceDbContext = productServiceDbContext.IfEmptyThenThrowAndReturnValue();
        }

        public Maybe<IEnumerable<IDomainEvent>> GetUncommittedEvents()
        {
            var entities = GetEntities();

            var domainEvents = entities.HasNoValue || entities.Value.IsEmpty()
                ? new List<IDomainEvent>()
                : entities.Value.SelectMany(x => x.Entity.DomainEvents
                    .OrderBy(de => de.OccuredAt));

            return domainEvents.ToList();
        }

        public void DispatchEvents(IEnumerable<IDomainEvent> domainEvents)
        {
            var tasks = new List<Task>();
            var domainEventsList = domainEvents.ToList();

            if (domainEventsList.IsEmpty())
            {
                return;
            }

            foreach (var domainEvent in domainEventsList)
            {
                if (domainEvent == null)
                {
                    throw new DomainEventCanNotBeEmptyException(
                        "Domain event can not be null.");
                }

                tasks.Add(_domainEventPublisher.PublishAsync(domainEvent));
            }

            Task.WaitAll(tasks.ToArray(), default(CancellationToken));
        }

        public void ClearAllDomainEvents()
        {
            var entities = GetEntities();

            if (entities.HasNoValue || entities.Value.IsEmpty())
            {
                return;
            }

            entities.Value.ForEach(entity => entity.Entity.ClearDomainEvents());
        }

        private Maybe<List<EntityEntry<IEventSourcingEntity>>> GetEntities()
        {
            return _productServiceDbContext.ChangeTracker
                .Entries<IEventSourcingEntity>()
                .Where(e => e.Entity.DomainEvents.IsNotEmpty())
                .ToList();
        }
    }
}