namespace Shoppingendly.Services.Products.Core.Exceptions.Categories
{
    public class InvalidCategoryDescriptionException : ShoppingendlyException
    {
        public InvalidCategoryDescriptionException(string message) : base(message)
        {
        }
    }
}