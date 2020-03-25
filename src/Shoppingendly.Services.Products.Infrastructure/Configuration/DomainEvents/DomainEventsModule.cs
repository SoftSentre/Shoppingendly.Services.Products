using Autofac;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.DomainEvents;
using Module = Autofac.Module;

namespace Shoppingendly.Services.Products.Infrastructure.Configuration.DomainEvents
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