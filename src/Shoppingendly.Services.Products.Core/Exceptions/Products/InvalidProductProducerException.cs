using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    [Serializable]
    public class InvalidProductProducerException : ShoppingendlyException
    {
        public InvalidProductProducerException()
        {
        }

        public InvalidProductProducerException(string message) : base(message)
        {
        }

        public InvalidProductProducerException(string message, params object[] args) : base(message, args)
        {
        }

        protected InvalidProductProducerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidProductProducerException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }
        
        protected InvalidProductProducerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}