using System.Threading.Tasks;
using Shoppingendly.Services.Products.Application.CQRS.Base.Commands;
using Shoppingendly.Services.Products.Application.CQRS.Base.Results;

namespace Shoppingendly.Services.Products.Application.CQRS.Base.Bus
{
    public interface ICommandBus
    {
        Task<ICommandResult> SendAsync<TCommand>(TCommand command) 
            where TCommand : class, ICommand;
    }
}