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

using System.Threading.Tasks;
using Autofac;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Core.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly ILifetimeScope _lifetimeScope;

        public DomainEventPublisher(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope.IfEmptyThenThrowAndReturnValue();
        }

        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IDomainEvent
        {
            var domainEventHandler = _lifetimeScope
                .ResolveOptional<IDomainEventHandler<TEvent>>();

            if (domainEventHandler == null)
            {
                throw new PublishDomainEventFailed(
                    $"Unable to publish domain event {@event}.");
            }

            await domainEventHandler.HandleAsync(@event);
        }
    }
}