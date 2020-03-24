using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Configuration.Extensions
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