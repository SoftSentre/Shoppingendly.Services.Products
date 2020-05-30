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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Core.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Core.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Decorators
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
                _logger.LogInformation($"Processing domain event: {@event.GetType().FullName} ({@event})");
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