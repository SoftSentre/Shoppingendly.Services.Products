using Shoppingendly.Services.Products.Core.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions
{
    public class CommandPublishedFailedException : ShoppingendlyException
    {
        public CommandPublishedFailedException(string message) : base(message)
        {
        }
    }
}