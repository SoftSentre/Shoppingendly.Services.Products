using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    [Serializable]
    public class EmptyCategoryProvidedException : ShoppingendlyException
    {
        public EmptyCategoryProvidedException()
        {
        }

        public EmptyCategoryProvidedException(string message) : base(message)
        {
        }

        public EmptyCategoryProvidedException(string message, params object[] args) : base(message, args)
        {
        }

        protected EmptyCategoryProvidedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EmptyCategoryProvidedException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected EmptyCategoryProvidedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}