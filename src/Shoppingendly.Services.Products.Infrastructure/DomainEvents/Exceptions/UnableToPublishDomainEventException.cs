using Shoppingendly.Services.Products.Core.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions
{
    public class UnableToPublishDomainEventException : ShoppingendlyException
    {
        public UnableToPublishDomainEventException(string message) : base(message)
        {
        }
    }
}