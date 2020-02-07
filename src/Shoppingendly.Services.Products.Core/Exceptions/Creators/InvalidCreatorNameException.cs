using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    [Serializable]
    public class InvalidCreatorNameException : ShoppingendlyException
    {
        public InvalidCreatorNameException()
        {
        }

        public InvalidCreatorNameException(string message) : base(message)
        {
        }

        public InvalidCreatorNameException(string message, params object[] args) : base(message, args)
        {
        }

        protected InvalidCreatorNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidCreatorNameException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected InvalidCreatorNameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}