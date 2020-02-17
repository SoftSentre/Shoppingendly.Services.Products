using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    [Serializable]
    public class ProductWithAssignedCategoryNotFoundException : ShoppingendlyException
    {
        public ProductWithAssignedCategoryNotFoundException()
        {
        }

        public ProductWithAssignedCategoryNotFoundException(string message) : base(message)
        {
        }

        public ProductWithAssignedCategoryNotFoundException(string message, params object[] args) : base(message, args)
        {
        }

        protected ProductWithAssignedCategoryNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ProductWithAssignedCategoryNotFoundException(string format, Exception innerException,
            params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected ProductWithAssignedCategoryNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}