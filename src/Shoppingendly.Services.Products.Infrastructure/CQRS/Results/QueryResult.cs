using System.Collections.Generic;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Results
{
    public class QueryResult<T> : Result, IQueryResult<T>
    {
        public T Data { get; private set; }

        public static QueryResult<T> Success(T data)
        {
            return new QueryResult<T> {Ok = true, Data = data};
        }

        public static QueryResult<T> Failed(string error, ErrorType errorType)
        {
            return new QueryResult<T> {Ok = false, ErrorType = errorType, Message = error};
        }

        public static QueryResult<T> Failed(IDictionary<string, string> errors, ErrorType errorType)
        {
            return new QueryResult<T> {Ok = false, ErrorType = errorType, Errors = errors};
        }
    }
}