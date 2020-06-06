// Copyright 2020 SoftSentre Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Base.Commands;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Base.Results;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.CQRS.Commands
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
                _logger.LogInformation($"Processing command: {command.GetGenericTypeName()} ({command})");
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