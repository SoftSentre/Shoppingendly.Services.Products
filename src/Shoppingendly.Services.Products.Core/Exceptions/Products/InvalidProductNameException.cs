namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class InvalidProductNameException : ShoppingendlyException
    {
        public InvalidProductNameException(string message) : base(message)
        {
        }
    }
}