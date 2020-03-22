namespace Shoppingendly.Services.Products.Core.Exceptions.Services.Products
{
    internal class ProductNotFoundException : ShoppingendlyException
    {
        internal ProductNotFoundException(string message) : base(message)
        {
        }
    }
}