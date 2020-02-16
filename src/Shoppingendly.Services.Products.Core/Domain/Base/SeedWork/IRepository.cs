using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Base.SeedWork
{
    public interface IRepository<TEntity, in TId>
        where TEntity : class, IEntity<TId>
    {
        Task<Maybe<TEntity>> GetById(TId id, string[] includeProperties = null);

        Task<Maybe<TEntity>> FindSpecific(Expression<Func<TEntity, bool>> predicate, 
            string[] includeProperties = null);

        Task<bool> Exist(TId id);
        Task<bool> Exist(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> Count();
        Task<int> Count(Func<TEntity, bool> predicate);

        Task<Maybe<IEnumerable<TEntity>>> FindOrGetAll(
            Expression<Func<TEntity, bool>> predicate = null,
            string[] includeProperties = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            int? skip = null, int? take = null);

        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entityCollection);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
        Task DeleteRange(IEnumerable<TEntity> entityCollection);
        Task Delete(TId id);
    }
}