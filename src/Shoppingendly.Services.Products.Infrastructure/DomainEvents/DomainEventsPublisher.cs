using System.Threading.Tasks;
using Autofac;
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
        private readonly IComponentContext _componentContext;
        private readonly ILogger _logger;

        public DomainEventsPublisher(IComponentContext componentContext, ILogger logger)
        {
            _componentContext = componentContext.IfEmptyThenThrowAndReturnValue();
            _logger = logger.IfEmptyThenThrowAndReturnValue();
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            var domainEventHandler = _componentContext.ResolveOptional<IDomainEventHandler<TEvent>>();

            if (domainEventHandler == null)
                throw new UnableToPublishDomainEventException(
                    $"Unable to publish domain event {@event}.");

            _logger.LogInformation(Smart.Format(
                $"Publishing domain event: {@event} to appropriate handler."));
            
            await domainEventHandler.HandleAsync(@event);
        }
    }
}