﻿// Copyright 2020 SoftSentre Contributors
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

using Autofac;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.DomainEvents;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.DomainEvents
{
    public class DomainEventsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEventsEfAccessor>()
                .As<IDomainEventAccessor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventPublisher>()
                .As<IDomainEventPublisher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();
        }
    }
}