using System.Threading.Tasks;
using Shoppingendly.Services.Products.Application.CQRS.Base.Results;

namespace Shoppingendly.Services.Products.Application.CQRS.Base.Queries
{
    public interface IQueryHandler<in TQuery,TResult> 
        where TQuery : class, IQuery<TResult>
    {
        Task<IQueryResult<TResult>> QueryAsync(TQuery query);
    }
}