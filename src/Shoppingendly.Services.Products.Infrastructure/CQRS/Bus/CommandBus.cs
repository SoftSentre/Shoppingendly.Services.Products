using System.Threading.Tasks;
using Autofac;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Commands;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Bus
{
    public class CommandBus : ICommandBus
    {
        private readonly ILifetimeScope _lifetimeScope;

        public CommandBus(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope.IfEmptyThenThrowAndReturnValue();
        }

        public async Task<ICommandResult> SendAsync<TCommand>(TCommand command) 
            where TCommand : class, ICommand
        {
            await using var scope = _lifetimeScope.BeginLifetimeScope();
            var commandHandler = scope.ResolveOptional<ICommandHandler<TCommand>>();

            if (commandHandler == null)
                throw new CommandPublishedFailedException(
                    $"Unable to publish command: {command.GetType().Name}");

            return await commandHandler.SendAsync(command);
        }
    }
}