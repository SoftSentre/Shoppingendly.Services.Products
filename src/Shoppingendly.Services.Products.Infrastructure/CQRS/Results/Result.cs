using System.Collections.Generic;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Results
{
    public class Result : IResult
    {
        public bool Ok { get; set; }
        
        public ErrorType ErrorType { get; set; }

        public IDictionary<string, string> Errors { get; set; }

        public string Message { get; set; }
    }
}