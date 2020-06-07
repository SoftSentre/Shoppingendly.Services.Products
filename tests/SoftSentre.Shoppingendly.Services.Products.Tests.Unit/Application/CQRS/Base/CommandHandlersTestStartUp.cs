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

using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Base.Commands;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.CQRS.Commands;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Application.CQRS.Base
{
    public class CommandHandlersTestStartUp<TCommand> : IAsyncLifetime where TCommand : class, ICommand
    {
        protected Mock<IValidator<TCommand>> CommandValidator;
        protected Mock<IDbContextTransaction> DbContextTransaction;
        protected Mock<ILogger<LoggingCommandHandlerDecorator<TCommand>>> Logger;
        protected Mock<IUnitOfWork> UnitOfWorkMock;

        public virtual async Task InitializeAsync()
        {
            Logger = new Mock<ILogger<LoggingCommandHandlerDecorator<TCommand>>>();
            DbContextTransaction = new Mock<IDbContextTransaction>();
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            CommandValidator = new Mock<IValidator<TCommand>>();

            await Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            Logger = null;
            DbContextTransaction = null;
            CommandValidator = null;
            UnitOfWorkMock = null;

            await Task.CompletedTask;
        }
    }
}