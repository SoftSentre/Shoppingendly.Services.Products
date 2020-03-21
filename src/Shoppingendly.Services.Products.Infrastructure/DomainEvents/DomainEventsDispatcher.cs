using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly ILogger<DomainEventsDispatcher> _logger;
        private readonly IDomainEventAccessor _domainEventAccessor;

        public DomainEventsDispatcher(
            ILogger<DomainEventsDispatcher> logger,
            IDomainEventAccessor domainEventAccessor)
        {
            _logger = logger.IfEmptyThenThrowAndReturnValue();
            _domainEventAccessor = domainEventAccessor.IfEmptyThenThrowAndReturnValue();
        }

        public async Task DispatchAsync()
        {
            try
            {
                var uncommittedEvents = _domainEventAccessor.GetUncommittedEvents();

                if (uncommittedEvents.HasNoValue || uncommittedEvents.Value.IsEmpty())
                {
                    _logger.LogInformation("In this transactional scope any domain event hasn't been produced.");
                    return;
                }

                _domainEventAccessor.DispatchEvents(uncommittedEvents.Value);
                _domainEventAccessor.ClearAllDomainEvents();
                await Task.CompletedTask;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error occured when dispatching the domain events. Message: {exception.Message}",
                    exception);
                throw new DispatchedDomainEventsFailedException(
                    $"Error occured when dispatching the domain events. Message: {exception.Message}", exception);
            }
        }
    }
}