namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    public class EmptyProductProvidedException : ShoppingendlyException
    {
        public EmptyProductProvidedException(string message) : base(message)
        {
        }
    }
}