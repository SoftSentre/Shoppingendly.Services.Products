using Autofac;
using Shoppingendly.Services.Products.Application.CQRS.Base.Bus;
using Shoppingendly.Services.Products.Application.CQRS.Base.Commands;
using Shoppingendly.Services.Products.Application.CQRS.Base.Queries;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Bus;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Commands;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Queries;

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

            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkCommandHandlerDecorator<>), 
                typeof(ICommandHandler<>));
            
            builder.RegisterGenericDecorator(
                typeof(ValidationCommandHandlerDecorator<>), 
                typeof(ICommandHandler<>));
            
            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerDecorator<>), 
                typeof(ICommandHandler<>));
            
            builder.RegisterGenericDecorator(
                typeof(LoggingQueryHandlerDecorator<,>), 
                typeof(IQueryHandler<,>));
        }
    }
}