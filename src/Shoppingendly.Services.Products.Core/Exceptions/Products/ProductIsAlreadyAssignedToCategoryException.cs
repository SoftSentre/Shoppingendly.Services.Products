using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    [Serializable]
    public class ProductIsAlreadyAssignedToCategoryException : ShoppingendlyException
    {
        public ProductIsAlreadyAssignedToCategoryException()
        {
        }

        public ProductIsAlreadyAssignedToCategoryException(string message) : base(message)
        {
        }

        public ProductIsAlreadyAssignedToCategoryException(string message, params object[] args) : base(message, args)
        {
        }

        protected ProductIsAlreadyAssignedToCategoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ProductIsAlreadyAssignedToCategoryException(string format, Exception innerException,
            params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected ProductIsAlreadyAssignedToCategoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}