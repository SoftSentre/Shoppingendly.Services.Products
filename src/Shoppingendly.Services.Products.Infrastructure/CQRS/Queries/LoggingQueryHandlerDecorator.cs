using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Core.Exceptions;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.CQRS.Results;
using SmartFormat;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Queries
{
    public class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decorated;
        private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> _logger;

        public LoggingQueryHandlerDecorator(
            IQueryHandler<TQuery, TResult> decorated,
            ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
        {
            _decorated = decorated.IfEmptyThenThrowAndReturnValue();
            _logger = logger.IfEmptyThenThrowAndReturnValue();
        }

        public async Task<IQueryResult<TResult>> QueryAsync(TQuery query)
        {
            IQueryResult<TResult> result;

            try
            {
                _logger.LogInformation(Smart.Format("Processing query:", query));
                result = await _decorated.QueryAsync(query);
                _logger.LogInformation(Smart.Format("Query processed with result:", result));
                return result;
            }
            catch (ShoppingendlyException shoppingendlyException)
            {
                _logger.LogError(Smart.Format(
                    $"Custom exception occured when processing a query. Message: {shoppingendlyException.Message}",
                    query));

                result = QueryResult<TResult>.Failed(shoppingendlyException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(Smart.Format(
                    $"Exception occured when processing a query. Message: {exception.Message}", query));

                result = QueryResult<TResult>.Failed(exception.Message);
            }

            return result;
        }
    }
}