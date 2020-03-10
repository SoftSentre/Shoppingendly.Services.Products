using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Core.Exceptions;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;
using SmartFormat;

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
                _logger.LogInformation(Smart.Format("Processing command:", command));
                result = await _decorated.SendAsync(command);
                _logger.LogInformation(Smart.Format("Command processed with result:", result));
                return result;
            }
            catch (InvalidCommandException invalidCommandException)
            {
                _logger.LogError(Smart.Format($"Command is invalid. Message: {invalidCommandException.Message}",
                    command));

                result = CommandResult.Failed(invalidCommandException.Message);
            }
            catch (ShoppingendlyException shoppingendlyException)
            {
                _logger.LogError(Smart.Format(
                    $"Custom exception occured when processing a command. Message: {shoppingendlyException.Message}",
                    command));

                result = CommandResult.Failed(shoppingendlyException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(Smart.Format(
                    $"Exception occured when processing a command. Message: {exception.Message}", command));

                result = CommandResult.Failed(exception.Message);
            }

            return result;
        }
    }
}