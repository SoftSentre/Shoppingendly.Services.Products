using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Services.Products
{
    [Serializable]
    public class EmptyProductProvidedException : ShoppingendlyException
    {
        public EmptyProductProvidedException()
        {
        }

        public EmptyProductProvidedException(string message) : base(message)
        {
        }

        public EmptyProductProvidedException(string message, params object[] args) : base(message, args)
        {
        }

        protected EmptyProductProvidedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EmptyProductProvidedException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected EmptyProductProvidedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}