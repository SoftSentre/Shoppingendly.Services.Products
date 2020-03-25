using System.Reflection;
using Autofac;
using Shoppingendly.Services.Products.Application.CQRS.Base.Commands;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Commands;
using Module = Autofac.Module;

namespace Shoppingendly.Services.Products.Infrastructure.Configuration.CQRS
{
    public class CommandHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var commandAssembly = typeof(ICommand)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(commandAssembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();
            
            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkCommandHandlerDecorator<>),
                typeof(ICommandHandler<>));

            builder.RegisterGenericDecorator(
                typeof(ValidationCommandHandlerDecorator<>),
                typeof(ICommandHandler<>));

            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerDecorator<>),
                typeof(ICommandHandler<>));
        }
    }
}