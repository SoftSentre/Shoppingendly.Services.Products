using System.Threading.Tasks;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Commands;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Bus
{
    public interface ICommandBus
    {
        Task<ICommandResult> SendAsync<TCommand>(TCommand command) 
            where TCommand : class, ICommand;
    }
}