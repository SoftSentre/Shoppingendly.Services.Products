using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Base.SeedWork;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductServiceDbContext _productServiceDbContext;

        public UnitOfWork(ProductServiceDbContext productServiceDbContext)
        {
            _productServiceDbContext = productServiceDbContext
                .IfEmptyThenThrowAndReturnValue();
        }

        public Task<bool> SaveAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}