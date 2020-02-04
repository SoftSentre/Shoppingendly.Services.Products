namespace Shoppingendly.Services.Products.Core.Exceptions
{
    public class InvalidCategoryNameException : ShoppingendlyException
    {
        public InvalidCategoryNameException(string message,  params object[] args) : base(message, args)
        {
        }
    }
}