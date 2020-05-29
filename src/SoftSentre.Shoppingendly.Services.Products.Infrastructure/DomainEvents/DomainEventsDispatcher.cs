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
using SoftSentre.Shoppingendly.Services.Products.Core.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IDomainEventAccessor _domainEventAccessor;
        private readonly ILogger<DomainEventsDispatcher> _logger;

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