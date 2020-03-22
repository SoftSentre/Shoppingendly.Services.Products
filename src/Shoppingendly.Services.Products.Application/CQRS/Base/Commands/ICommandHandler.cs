using System.Threading.Tasks;
using Shoppingendly.Services.Products.Application.CQRS.Base.Results;

namespace Shoppingendly.Services.Products.Application.CQRS.Base.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task<ICommandResult> SendAsync(TCommand command);
    }
}