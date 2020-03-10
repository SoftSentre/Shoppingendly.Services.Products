using Shoppingendly.Services.Products.Core.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions
{
    public class PublishDomainEventFailed : ShoppingendlyException
    {
        public PublishDomainEventFailed(string message) : base(message)
        {
        }
    }
}