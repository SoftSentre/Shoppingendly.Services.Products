using System.Threading.Tasks;
using Shoppingendly.Services.Products.Application.CQRS.Base.Queries;
using Shoppingendly.Services.Products.Application.CQRS.Base.Results;

namespace Shoppingendly.Services.Products.Application.CQRS.Base.Bus
{
    public interface IQueryBus
    {
        Task<IQueryResult<TResult>> QueryAsync<TResult>(IQuery<TResult> query);
        
        Task<IQueryResult<TResult>> QueryAsync<TQuery, TResult>(TQuery query) 
            where TQuery : class, IQuery<TResult>;
    }
}