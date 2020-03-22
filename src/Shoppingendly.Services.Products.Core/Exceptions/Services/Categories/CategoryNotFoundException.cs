namespace Shoppingendly.Services.Products.Core.Exceptions.Services.Categories
{
    internal class CategoryNotFoundException : ShoppingendlyException
    {
        internal CategoryNotFoundException(string message) : base(message)
        {
        }
    }
}