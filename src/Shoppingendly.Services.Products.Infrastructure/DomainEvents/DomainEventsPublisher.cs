using System.Threading.Tasks;
using Autofac;
using Autofac.Core.Lifetime;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions;
using SmartFormat;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents
{
    public class DomainEventsPublisher : IDomainEventPublisher
    {
        private readonly ILifetimeScope _lifetimeScope;

        public DomainEventsPublisher(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope.IfEmptyThenThrowAndReturnValue();
        }

        public async Task PublishAsync<TEvent>(TEvent @event) 
            where TEvent : class, IDomainEvent
        {
            var domainEventHandler = _lifetimeScope
                .ResolveOptional<IDomainEventHandler<TEvent>>();

            if (domainEventHandler == null)
                throw new PublishDomainEventFailed(
                    $"Unable to publish domain event {@event}.");

            await domainEventHandler.HandleAsync(@event);
        }
    }
}