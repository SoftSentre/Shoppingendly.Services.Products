namespace Shoppingendly.Services.Products.Core.Exceptions.Services.Categories
{
    internal class CategoryAlreadyExistsException : ShoppingendlyException
    {
        internal CategoryAlreadyExistsException(string message) : base(message)
        {
        }
    }
}