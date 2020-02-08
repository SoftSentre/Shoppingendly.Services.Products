using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    [Serializable]
    public class InvalidProductNameException : ShoppingendlyException
    {
        public InvalidProductNameException()
        {
        }

        public InvalidProductNameException(string message) : base(message)
        {
        }

        public InvalidProductNameException(string message, params object[] args) : base(message, args)
        {
        }

        protected InvalidProductNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidProductNameException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }
        
        protected InvalidProductNameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}