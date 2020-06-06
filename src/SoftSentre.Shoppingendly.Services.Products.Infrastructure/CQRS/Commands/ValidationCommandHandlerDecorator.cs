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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Base.Commands;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Base.Results;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.CQRS.Commands
{
    public class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _decorated;
        private readonly IList<IValidator<TCommand>> _validators;

        public ValidationCommandHandlerDecorator(
            ICommandHandler<TCommand> decorated,
            IList<IValidator<TCommand>> validators)
        {
            _decorated = decorated.IfEmptyThenThrowAndReturnValue();
            _validators = validators.IfEmptyThenThrowAndReturnValue();
        }

        public async Task<ICommandResult> SendAsync(TCommand command)
        {
            var errors = _validators
                .Select(v => v.Validate(command))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (!errors.Any())
            {
                return await _decorated.SendAsync(command);
            }

            var errorBuilder = new StringBuilder();
            errorBuilder.AppendLine("Invalid command, reason: ");

            foreach (var error in errors)
            {
                errorBuilder.AppendLine(error.ErrorMessage);
            }

            throw new InvalidCommandException(errorBuilder.ToString());
        }
    }
}