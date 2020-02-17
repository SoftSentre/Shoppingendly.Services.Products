namespace Shoppingendly.Services.Products.Core.Exceptions.Categories
{
    public class InvalidCategoryNameException : ShoppingendlyException
    {
        public InvalidCategoryNameException(string message, params object[] args) : base(message, args)
        {
        }
    }
}