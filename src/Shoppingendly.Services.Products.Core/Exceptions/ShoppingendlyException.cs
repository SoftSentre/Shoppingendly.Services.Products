using System;

namespace Shoppingendly.Services.Products.Core.Exceptions
{
    public abstract class ShoppingendlyException : Exception
    {
        protected ShoppingendlyException()
        {
        }
        
        protected ShoppingendlyException(string message) : base(message)
        {
        }

        protected ShoppingendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}