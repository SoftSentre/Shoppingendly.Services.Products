namespace Shoppingendly.Services.Products.Core.Exceptions.Categories
{
    internal class InvalidCategoryDescriptionException : ShoppingendlyException
    {
        internal InvalidCategoryDescriptionException(string message) : base(message)
        {
        }
    }
}