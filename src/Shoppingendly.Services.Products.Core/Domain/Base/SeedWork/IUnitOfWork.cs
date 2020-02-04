using System.Threading.Tasks;

namespace Shoppingendly.Services.Products.Core.Domain.Base.SeedWork
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();
    }
}