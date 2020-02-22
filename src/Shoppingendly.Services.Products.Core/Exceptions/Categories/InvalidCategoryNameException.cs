namespace Shoppingendly.Services.Products.Core.Exceptions.Categories
{
    public class InvalidCategoryNameException : ShoppingendlyException
    {
        public InvalidCategoryNameException(string message) : base(message)
        {
        }
    }
}