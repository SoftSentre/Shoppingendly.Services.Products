using System.Threading.Tasks;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Base
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchAsync();
    }
}