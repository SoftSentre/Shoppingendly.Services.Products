using Autofac;
using Shoppingendly.Services.Products.Application.CQRS.Base.Bus;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Bus;
using Module = Autofac.Module;

namespace Shoppingendly.Services.Products.Infrastructure.Configuration.CQRS
{
    public class CqrsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandBus>()
                .As<ICommandBus>()
                .InstancePerDependency();

            builder.RegisterType<QueryBus>()
                .As<IQueryBus>()
                .InstancePerDependency();
        }
    }
}