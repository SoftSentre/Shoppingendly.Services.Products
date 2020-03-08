using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Commands
{
    public class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _decorated;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<TCommand> decorated, 
            IUnitOfWork unitOfWork)
        {
            _decorated = decorated.IfEmptyThenThrowAndReturnValue();
            _unitOfWork = unitOfWork.IfEmptyThenThrowAndReturnValue();
        }

        public async Task<ICommandResult> HandleAsync(TCommand command)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            
            var result = await _decorated.HandleAsync(command);
            
            await _unitOfWork.CommitTransactionAsync(transaction);
            
            return result;
        }
    }
}