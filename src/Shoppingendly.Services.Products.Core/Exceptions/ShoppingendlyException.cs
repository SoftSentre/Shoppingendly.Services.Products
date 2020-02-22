using System;

namespace Shoppingendly.Services.Products.Core.Exceptions
{
    public abstract class ShoppingendlyException : Exception
    {
        protected ShoppingendlyException(string message) : base(message)
        {
        }
    }
}