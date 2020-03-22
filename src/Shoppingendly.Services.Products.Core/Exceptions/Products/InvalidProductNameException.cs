namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    internal class InvalidProductNameException : ShoppingendlyException
    {
        internal InvalidProductNameException(string message) : base(message)
        {
        }
    }
}