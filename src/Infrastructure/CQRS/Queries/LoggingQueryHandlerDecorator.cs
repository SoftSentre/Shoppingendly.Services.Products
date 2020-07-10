// Copyright 2020 SoftSentre Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.CQRS.Queries;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.CQRS.Results;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.CQRS.Queries
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
                _logger.LogInformation($"Processing command: {query.GetGenericTypeName()} ({query})");
                result = await _decorated.QueryAsync(query);
                _logger.LogInformation("Query successfully processed.");
            }
            catch (InternalException shoppingendlyException)
            {
                _logger.LogError(
                    $"Custom exception occured when processing a query. Message: {shoppingendlyException.Message}",
                    shoppingendlyException);

                result = QueryResult<TResult>.Failed(shoppingendlyException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception occured when processing a query. Message: {exception.Message}", exception);

                result = QueryResult<TResult>.Failed(exception.Message);
            }

            return result;
        }
    }
}