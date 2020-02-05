namespace Shoppingendly.Services.Products.Core.Exceptions
{
    public class InvalidCategoryDescriptionException : ShoppingendlyException
    {
        public InvalidCategoryDescriptionException(string message) : base(message)
        {
        }

        public InvalidCategoryDescriptionException(string message,  params object[] args) : base(message, args)
        {
            
        }
    }
}