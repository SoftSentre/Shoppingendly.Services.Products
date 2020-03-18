using System;
using Shoppingendly.Services.Products.Core.Exceptions;

namespace Shoppingendly.Services.Products.Infrastructure.DomainEvents.Exceptions
{
    public class DispatchedDomainEventsFailedException : ShoppingendlyException
    {
        public DispatchedDomainEventsFailedException(string message) : base(message)
        {
        }

        public DispatchedDomainEventsFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}