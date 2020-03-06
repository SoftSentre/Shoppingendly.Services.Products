using System.Threading.Tasks;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Queries
{
    public interface IQueryHandler<in TQuery,TResult> where TQuery : class, IQuery<TResult>
    {
        Task<IQueryResult<TResult>> HandleAsync(TQuery query);
    }
}