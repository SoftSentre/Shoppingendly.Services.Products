using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Base.SeedWork;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories.Base
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        private readonly ProductServiceDbContext _productServiceDbContext;

        protected Repository(ProductServiceDbContext productServiceDbContext)
        {
            _productServiceDbContext = productServiceDbContext
                .IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<TEntity>> GetById(TId id, string[] includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Maybe<TEntity>> FindSpecific(Expression<Func<TEntity, bool>> predicate,
            string[] includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(TId id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(Expression<Func<TEntity, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Count(Func<TEntity, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Maybe<IEnumerable<TEntity>>> FindOrGetAll(Expression<Func<TEntity, bool>> predicate = null,
            string[] includeProperties = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null, int? take = null)
        {
            throw new NotImplementedException();
        }

        public async Task Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task AddRange(IEnumerable<TEntity> entityCollection)
        {
            throw new NotImplementedException();
        }

        public async Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteRange(IEnumerable<TEntity> entityCollection)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(TId id)
        {
            throw new NotImplementedException();
        }
    }
}