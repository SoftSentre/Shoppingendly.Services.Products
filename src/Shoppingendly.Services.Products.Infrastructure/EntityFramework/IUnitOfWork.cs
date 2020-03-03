using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework
{
    public interface IUnitOfWork
    {
        IDbContextTransaction GetCurrentTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        void RollbackTransaction();
        Task<bool> SaveAsync();
    }
}