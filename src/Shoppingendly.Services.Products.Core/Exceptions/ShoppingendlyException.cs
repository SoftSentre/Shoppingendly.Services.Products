using System;

namespace Shoppingendly.Services.Products.Core.Exceptions
{
    public abstract class ShoppingendlyException : Exception
    {
        protected ShoppingendlyException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}