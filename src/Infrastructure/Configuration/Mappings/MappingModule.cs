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
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using SoftSentre.Shoppingendly.Services.Products.Application.Mapper;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.AutoMapper;
using Module = Autofac.Module;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Mappings
{
    public class MappingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblyNames = Assembly
                .GetExecutingAssembly()
                .GetReferencedAssemblies();

            var assembliesTypes = assemblyNames
                .Where(a => a.Name.StartsWith("Shoppingendly.Services.Products.Application",
                    StringComparison.OrdinalIgnoreCase))
                .SelectMany(an => Assembly.Load(an).GetTypes())
                .Where(p => typeof(Profile).IsAssignableFrom(p) && p.IsPublic && !p.IsAbstract)
                .Distinct();

            var autoMapperProfiles = assembliesTypes
                .Select(p => (Profile) Activator.CreateInstance(p))
                .ToList();

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (var profile in autoMapperProfiles)
                {
                    cfg.AddProfile(profile);
                }
            }));

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper())
                .As<IMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MapperWrapper>()
                .As<IMapperWrapper>()
                .InstancePerLifetimeScope();
        }
    }
}