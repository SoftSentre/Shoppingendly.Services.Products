namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    internal class EmptyCreatorProvidedException : ShoppingendlyException
    {
        internal EmptyCreatorProvidedException(string message) : base(message)
        {
        }
    }
}