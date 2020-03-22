using System;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Shoppingendly.Services.Products.Application.Mapper;
using Shoppingendly.Services.Products.Infrastructure.AutoMapper;
using Module = Autofac.Module;

namespace Shoppingendly.Services.Products.Infrastructure.Configuration.Mapping
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