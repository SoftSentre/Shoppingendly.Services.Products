using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    [Serializable]
    public class AnyProductWithAssignedCategoryNotFoundException : ShoppingendlyException
    {
        public AnyProductWithAssignedCategoryNotFoundException()
        {
        }

        public AnyProductWithAssignedCategoryNotFoundException(string message) : base(message)
        {
        }

        public AnyProductWithAssignedCategoryNotFoundException(string message, params object[] args) : base(message,
            args)
        {
        }

        protected AnyProductWithAssignedCategoryNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AnyProductWithAssignedCategoryNotFoundException(string format, Exception innerException,
            params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected AnyProductWithAssignedCategoryNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}