namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class InvalidProductProducerException : ShoppingendlyException
    {
        public InvalidProductProducerException(string message, params object[] args) : base(message, args)
        {
        }
    }
}