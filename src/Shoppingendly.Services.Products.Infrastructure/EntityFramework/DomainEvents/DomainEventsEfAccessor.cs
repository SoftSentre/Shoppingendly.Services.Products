using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IDomainEventPublisher _domainEventBus;
        private readonly ProductServiceDbContext _productServiceDbContext;

        public DomainEventsEfAccessor(
            IDomainEventPublisher domainEventBus,
            ProductServiceDbContext productServiceDbContext)
        {
            _domainEventBus = domainEventBus.IfEmptyThenThrowAndReturnValue();
            _productServiceDbContext = productServiceDbContext.IfEmptyThenThrowAndReturnValue();
        }

        public Maybe<IEnumerable<IDomainEvent>> GetUncommitted()
            => GetDomainEvents();

        public async Task DispatchEventsAsync()
        {
            var domainEvents = GetDomainEvents();

            if (domainEvents.HasNoValue || domainEvents.Value.IsEmpty())
                return;

            foreach (var domainEvent in domainEvents.Value)
            {
                if (domainEvent == null)
                    throw new DomainEventCanNotBeEmptyException("Domain event can not be null.");

                await _domainEventBus.PublishAsync(domainEvent);
            }
        }

        public void ClearAllDomainEvents()
        {
            var entities = _productServiceDbContext.ChangeTracker
                .Entries<AuditableAndEventSourcingEntity<Identity<Guid>>>()
                .Where(e => e.Entity.DomainEvents.IsNotEmpty())
                .ToList();

            if (entities.IsEmpty())
                return;

            entities.ForEach(entity => entity.Entity.ClearDomainEvents());
        }

        private Maybe<IEnumerable<IDomainEvent>> GetDomainEvents()
        {
            var entities = _productServiceDbContext.ChangeTracker
                .Entries<AuditableAndEventSourcingEntity<Identity<Guid>>>()
                .Where(e => e.Entity.DomainEvents.IsNotEmpty())
                .ToList();

            var domainEvents = entities.IsEmpty()
                ? new List<IDomainEvent>()
                : entities.SelectMany(x => x.Entity.DomainEvents);

            return domainEvents.ToList();
        }
    }
}