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

using System.Reflection;
using Autofac;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Base.Queries;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.CQRS.Queries;
using Module = Autofac.Module;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.CQRS
{
    public class QueryHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var queryAssembly = typeof(IQuery)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(queryAssembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(
                typeof(LoggingQueryHandlerDecorator<,>),
                typeof(IQueryHandler<,>));
        }
    }
}