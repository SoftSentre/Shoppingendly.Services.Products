using System.Reflection;
using Autofac;
using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Infrastructure.DomainEvents.Decorators;
using Module = Autofac.Module;

namespace Shoppingendly.Services.Products.Infrastructure.Configuration.DomainEvents
{
    public class DomainEventHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var domainEventsAssembly =  typeof(IDomainEvent)
                .GetTypeInfo()
                .Assembly;
            
            builder.RegisterAssemblyTypes(domainEventsAssembly)
                .AsClosedTypesOf(typeof(IDomainEventHandler<>))
                .InstancePerLifetimeScope();
            
            builder.RegisterGenericDecorator(
                typeof(LoggingDomainEventHandlerDecorator<>),
                typeof(IDomainEventHandler<>));
        }
    }
}