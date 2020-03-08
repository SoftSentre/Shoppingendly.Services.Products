using Shoppingendly.Services.Products.Core.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions
{
    public class InvalidCommandException : ShoppingendlyException
    {
        public InvalidCommandException(string message) : base(message)
        {
        }
    }
}