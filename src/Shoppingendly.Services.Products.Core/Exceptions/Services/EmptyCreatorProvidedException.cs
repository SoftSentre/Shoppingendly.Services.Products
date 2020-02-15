using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    [Serializable]
    public class EmptyCreatorProvidedException : ShoppingendlyException
    {
        public EmptyCreatorProvidedException()
        {
        }

        public EmptyCreatorProvidedException(string message) : base(message)
        {
        }

        public EmptyCreatorProvidedException(string message, params object[] args) : base(message, args)
        {
        }

        protected EmptyCreatorProvidedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EmptyCreatorProvidedException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected EmptyCreatorProvidedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}