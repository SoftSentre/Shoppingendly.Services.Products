using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions
{
    [Serializable]
    public abstract class ShoppingendlyException : Exception
    {
        protected ShoppingendlyException()
        {
        }

        protected ShoppingendlyException(string message)
            : base(message)
        {
        }

        protected ShoppingendlyException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        protected ShoppingendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ShoppingendlyException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected ShoppingendlyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}