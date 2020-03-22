namespace Shoppingendly.Services.Products.Core.Exceptions.Services.Products
{
    internal class ProductAlreadyExistsException : ShoppingendlyException
    {
        internal ProductAlreadyExistsException(string message) : base(message)
        {
        }
    }
}