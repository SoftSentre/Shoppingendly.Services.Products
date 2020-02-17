namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    public class EmptyProductProvidedException : ShoppingendlyException
    {
        public EmptyProductProvidedException(string message, params object[] args) : base(message, args)
        {
        }
    }
}