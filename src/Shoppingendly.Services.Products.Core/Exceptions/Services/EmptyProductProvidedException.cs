namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    internal class EmptyProductProvidedException : ShoppingendlyException
    {
        internal EmptyProductProvidedException(string message) : base(message)
        {
        }
    }
}