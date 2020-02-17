namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class InvalidProductNameException : ShoppingendlyException
    {
        public InvalidProductNameException(string message, params object[] args) : base(message, args)
        {
        }
    }
}