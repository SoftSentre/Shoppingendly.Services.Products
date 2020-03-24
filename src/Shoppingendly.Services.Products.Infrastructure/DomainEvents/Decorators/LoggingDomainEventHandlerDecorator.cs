using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Exceptions;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Decorators
{
    public class LoggingDomainEventHandlerDecorator<TEvent> : IDomainEventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        private readonly IDomainEventHandler<TEvent> _decorated;
        private readonly ILogger<LoggingDomainEventHandlerDecorator<TEvent>> _logger;

        public LoggingDomainEventHandlerDecorator(
            IDomainEventHandler<TEvent> decorated,
            ILogger<LoggingDomainEventHandlerDecorator<TEvent>> logger)
        {
            _decorated = decorated.IfEmptyThenThrowAndReturnValue();
            _logger = logger.IfEmptyThenThrowAndReturnValue();
        }

        public async Task HandleAsync(TEvent @event)
        {
            try
            {
                _logger.LogInformation("Processing domain event: {EventName} ({@Event})", @event.GetType().FullName,
                    @event);
                await _decorated.HandleAsync(@event);
                _logger.LogInformation("Domain event successfully processed.");
            }
            catch (ShoppingendlyException shoppingendlyException)
            {
                _logger.LogError(
                    $"Custom exception occured when processing a domain event. Message: {shoppingendlyException.Message}",
                    @event);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception occured when processing a domain event. Message: {exception.Message}",
                    @event);
            }
        }
    }
}