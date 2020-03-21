using Shoppingendly.Services.Products.Core.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions
{
    public class DomainEventCanNotBeEmptyException : ShoppingendlyException
    {
        public DomainEventCanNotBeEmptyException()
        {
        }
        
        public DomainEventCanNotBeEmptyException(string message) : base(message)
        {
        }
    }
}

