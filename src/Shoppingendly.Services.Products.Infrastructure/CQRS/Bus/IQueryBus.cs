using System.Threading.Tasks;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Commands;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Queries;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Bus
{
    public interface IQueryBus
    {
        Task<IQueryResult<TResult>> QueryAsync<TResult>(IQuery<TResult> query);
        
        Task<IQueryResult<TResult>> QueryAsync<TQuery, TResult>(TQuery query) 
            where TQuery : class, IQuery<TResult>;
    }
}