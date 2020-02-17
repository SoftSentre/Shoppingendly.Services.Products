using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Categories
{
    [Serializable]
    public class InvalidCategoryNameException : ShoppingendlyException
    {
        public InvalidCategoryNameException()
        {
        }

        public InvalidCategoryNameException(string message) : base(message)
        {
        }

        public InvalidCategoryNameException(string message, params object[] args) : base(message, args)
        {
        }

        protected InvalidCategoryNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidCategoryNameException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }
        
        protected InvalidCategoryNameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}