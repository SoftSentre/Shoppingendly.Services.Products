using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Categories
{
    [Serializable]
    public class InvalidCategoryDescriptionException : ShoppingendlyException
    {
        public InvalidCategoryDescriptionException()
        {
        }

        public InvalidCategoryDescriptionException(string message) : base(message)
        {
        }

        public InvalidCategoryDescriptionException(string message, params object[] args) : base(message, args)
        {
        }

        protected InvalidCategoryDescriptionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidCategoryDescriptionException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }
        
        protected InvalidCategoryDescriptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}