using System.Reflection;
using Autofac;
using Shoppingendly.Services.Products.Application.CQRS.Base.Queries;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Queries;
using Module = Autofac.Module;

namespace Shoppingendly.Services.Products.Infrastructure.Configuration.CQRS
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