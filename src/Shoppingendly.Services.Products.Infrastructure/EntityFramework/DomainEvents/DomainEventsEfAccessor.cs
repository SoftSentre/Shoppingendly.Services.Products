using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.DomainEvents
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
                return;

            foreach (var domainEvent in domainEventsList)
            {
                if (domainEvent == null)
                    throw new DomainEventCanNotBeEmptyException(
                        "Domain event can not be null.");

                tasks.Add(_domainEventPublisher.PublishAsync(domainEvent));
            }

            Task.WaitAll(tasks.ToArray(), default(CancellationToken));
        }

        public void ClearAllDomainEvents()
        {
            var entities = GetEntities();

            if (entities.HasNoValue || entities.Value.IsEmpty())
                return;

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