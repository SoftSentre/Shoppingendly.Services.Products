namespace Shoppingendly.Services.Products.Core.Exceptions.Categories
{
    internal class InvalidCategoryNameException : ShoppingendlyException
    {
        internal InvalidCategoryNameException(string message) : base(message)
        {
        }
    }
}