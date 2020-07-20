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

using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Services
{
    public class DomainEventsEmitter : IDomainEventEmitter
    {
        public void Emit<TEventSourcingEntity, TEvent>(TEventSourcingEntity eventSourcingEntity, TEvent domainEvent)
            where TEventSourcingEntity : class, IEventSourcingEntity
            where TEvent : class, IDomainEvent
        {
            if (eventSourcingEntity == null || domainEvent == null)
            {
                throw new EmitDomainEventFailedException("Parameters can not be a nulls.");
            }

            if (eventSourcingEntity.DomainEvents == null)
            {
                throw new EmitDomainEventFailedException(
                    $"Domain events list in entity: {eventSourcingEntity.GetGenericTypeName()} is not initialized.");
            }

            eventSourcingEntity.DomainEvents.Add(domainEvent);
        }
    }
}