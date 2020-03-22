using System.Threading.Tasks;
using Autofac;
using Shoppingendly.Services.Products.Application.CQRS.Base.Bus;
using Shoppingendly.Services.Products.Application.CQRS.Base.Queries;
using Shoppingendly.Services.Products.Application.CQRS.Base.Results;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions;

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
                    $"Can not send query: {query.GetType().Name}.");

            return await queryHandler.QueryAsync((dynamic) query);
        }

        public async Task<IQueryResult<TResult>> QueryAsync<TQuery, TResult>(TQuery query)
            where TQuery : class, IQuery<TResult>
        {
            await using var scope = _lifetimeScope.BeginLifetimeScope();

            var queryHandler = scope.ResolveOptional<IQueryHandler<TQuery, TResult>>();

            if (queryHandler == null)
                throw new SendingQueryFailedException(
                    $"Can not send query: {query.GetType().Name}.");

            return await queryHandler.QueryAsync(query);
        }
    }
}