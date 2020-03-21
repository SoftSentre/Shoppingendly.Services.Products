using Shoppingendly.Services.Products.Core.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Exceptions
{
    public class SendingQueryFailedException : ShoppingendlyException
    {
        public SendingQueryFailedException(string message) : base(message)
        {
        }
    }
}