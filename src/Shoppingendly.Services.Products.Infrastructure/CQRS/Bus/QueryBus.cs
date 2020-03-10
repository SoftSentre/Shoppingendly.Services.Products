using System.Threading.Tasks;
using Autofac;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Queries;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;
using SmartFormat;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Bus
{
    public class QueryBus : IQueryBus
    {
        private readonly ILifetimeScope _lifetimeScope;

        public QueryBus(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope.IfEmptyThenThrowAndReturnValue();
        }

        public async Task<IQueryResult<TResult>> QueryAsync<TResult>(IQuery<TResult> query)
        {
            await using var scope = _lifetimeScope.BeginLifetimeScope();

            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));

            dynamic queryHandler = scope.ResolveOptional(handlerType);

            if (queryHandler == null)
                throw new SendingQueryFailedException(
                    Smart.Format($"Can not send query: {query}."));

            return await queryHandler.QueryAsync((dynamic) query);
        }

        public async Task<IQueryResult<TResult>> QueryAsync<TQuery, TResult>(TQuery query)
            where TQuery : class, IQuery<TResult>
        {
            await using var scope = _lifetimeScope.BeginLifetimeScope();

            var queryHandler = scope.ResolveOptional<IQueryHandler<TQuery, TResult>>();

            if (queryHandler == null)
                throw new SendingQueryFailedException(
                    Smart.Format($"Can not send query: {query}."));

            return await queryHandler.QueryAsync(query);
        }
    }
}