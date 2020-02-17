namespace Shoppingendly.Services.Products.Core.Exceptions.Categories
{
    public class InvalidCategoryDescriptionException : ShoppingendlyException
    {
        public InvalidCategoryDescriptionException(string message, params object[] args) : base(message, args)
        {
        }
    }
}