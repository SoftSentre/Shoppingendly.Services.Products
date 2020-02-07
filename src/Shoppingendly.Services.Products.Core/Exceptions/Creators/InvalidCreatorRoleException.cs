using System;
using System.Runtime.Serialization;

namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    [Serializable]
    public class InvalidCreatorRoleException : ShoppingendlyException
    {
        public InvalidCreatorRoleException()
        {
        }

        public InvalidCreatorRoleException(string message) : base(message)
        {
        }

        public InvalidCreatorRoleException(string message, params object[] args) : base(message, args)
        {
        }

        protected InvalidCreatorRoleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidCreatorRoleException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }
        
        protected InvalidCreatorRoleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}