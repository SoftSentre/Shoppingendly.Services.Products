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

using System.Collections.Generic;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.DomainEvents;

namespace SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.Entities
{
    public abstract class EventSourcingEntity : EntityBase, IEventSourcingEntity
    {
        private List<IDomainEvent> _domainEvents;

        public IEnumerable<IDomainEvent> DomainEvents
            => _domainEvents.AsReadOnly();

        public IEnumerable<IDomainEvent> GetUncommitted()
        {
            return _domainEvents ??= new List<IDomainEvent>();
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}