using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Application.CQRS.Base.Commands;
using Shoppingendly.Services.Products.Application.CQRS.Base.Results;
using Shoppingendly.Services.Products.Core.Exceptions;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions;
using Shoppingendly.Services.Products.Infrastructure.Extensions;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Commands
{
    public class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _decorated;
        private readonly ILogger<LoggingCommandHandlerDecorator<TCommand>> _logger;

        public LoggingCommandHandlerDecorator(
            ICommandHandler<TCommand> decorated,
            ILogger<LoggingCommandHandlerDecorator<TCommand>> logger)
        {
            _decorated = decorated.IfEmptyThenThrowAndReturnValue();
            _logger = logger.IfEmptyThenThrowAndReturnValue();
        }

        public async Task<ICommandResult> SendAsync(TCommand command)
        {
            ICommandResult result;

            try
            {
                _logger.LogInformation("Processing command: {CommandName} ({@Command})", command.GetGenericTypeName(),
                    command);
                result = await _decorated.SendAsync(command);
                _logger.LogInformation("Command successfully processed.");
            }
            catch (InvalidCommandException invalidCommandException)
            {
                _logger.LogError($"Command is invalid. Message: {invalidCommandException.Message}",
                    invalidCommandException);

                result = CommandResult.Failed(invalidCommandException.Message);
            }
            catch (ShoppingendlyException shoppingendlyException)
            {
                _logger.LogError(
                    $"Custom exception occured when processing a command. Message: {shoppingendlyException.Message}",
                    shoppingendlyException);

                result = CommandResult.Failed(shoppingendlyException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception occured when processing a command. Message: {exception.Message}",
                    exception);

                result = CommandResult.Failed(exception.Message);
            }

            return result;
        }
    }
}