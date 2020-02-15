using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Services.Products
{
    [Serializable]
    public class AssignedCategoryNotFoundException : ShoppingendlyException
    {
        public AssignedCategoryNotFoundException()
        {
        }

        public AssignedCategoryNotFoundException(string message) : base(message)
        {
        }

        public AssignedCategoryNotFoundException(string message, params object[] args) : base(message, args)
        {
        }

        protected AssignedCategoryNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AssignedCategoryNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected AssignedCategoryNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}