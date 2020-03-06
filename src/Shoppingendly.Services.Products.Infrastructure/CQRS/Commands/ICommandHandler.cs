using System.Threading.Tasks;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task<ICommandResult> HandleAsync(TCommand command);
    }
}