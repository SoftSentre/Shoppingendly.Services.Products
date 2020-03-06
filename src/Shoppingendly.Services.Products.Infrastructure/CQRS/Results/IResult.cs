using System.Collections.Generic;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Results
{
    public interface IResult
    {
        bool Ok { get; }
        
        ErrorType ErrorType { get; }

        string Message { get; }

        IDictionary<string, string> Errors { get; }
    }
}