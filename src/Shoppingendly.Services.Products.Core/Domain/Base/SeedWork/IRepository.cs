using Shoppingendly.Services.Products.Core.Domain.Base.Entities;

namespace Shoppingendly.Services.Products.Core.Domain.Base.SeedWork
{
    public interface IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
    }
}