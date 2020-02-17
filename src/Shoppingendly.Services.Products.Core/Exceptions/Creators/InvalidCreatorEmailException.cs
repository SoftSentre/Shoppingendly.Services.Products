using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    [Serializable]
    public class InvalidCreatorEmailException : ShoppingendlyException
    {
        public InvalidCreatorEmailException()
        {
        }

        public InvalidCreatorEmailException(string message) : base(message)
        {
        }

        public InvalidCreatorEmailException(string message, params object[] args) : base(message, args)
        {
        }

        protected InvalidCreatorEmailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidCreatorEmailException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }
        
        protected InvalidCreatorEmailException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}