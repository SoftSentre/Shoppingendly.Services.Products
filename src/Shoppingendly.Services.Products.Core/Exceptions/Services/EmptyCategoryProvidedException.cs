namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    internal class EmptyCategoryProvidedException : ShoppingendlyException
    {
        internal EmptyCategoryProvidedException(string message) : base(message)
        {
        }
    }
}