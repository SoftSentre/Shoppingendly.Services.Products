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
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions
{
    public static class ModuleExtensions
    {
        public static IEnumerable<Type> GetTypesRegisteredInModule(this IModule module)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(module);
            var container = containerBuilder.Build();
            var componentRegistry = container.ComponentRegistry;
            
            var typesRegistered =
                componentRegistry.Registrations.SelectMany(x => x.Services)
                    .Cast<TypedService>()
                    .Select(x => x.ServiceType);

            return typesRegistered;
        }
    }
}