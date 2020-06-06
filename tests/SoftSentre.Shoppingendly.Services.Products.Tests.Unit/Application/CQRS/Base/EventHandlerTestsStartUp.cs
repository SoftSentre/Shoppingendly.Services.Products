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
using Microsoft.Extensions.Logging;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Decorators;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Application.CQRS.Base
{
    public class EventHandlerTestsStartUp<TEvent> : IAsyncLifetime where TEvent : class, IDomainEvent
    {
        protected Mock<ILogger<LoggingDomainEventHandlerDecorator<TEvent>>> Logger;
        
        public virtual async Task InitializeAsync()
        {
            Logger = new Mock<ILogger<LoggingDomainEventHandlerDecorator<TEvent>>>();

            await Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            Logger = null;

            await Task.CompletedTask;
        }
    }
}