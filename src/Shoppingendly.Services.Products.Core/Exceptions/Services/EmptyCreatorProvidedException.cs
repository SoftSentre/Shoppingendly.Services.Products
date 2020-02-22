namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    public class EmptyCreatorProvidedException : ShoppingendlyException
    {
        public EmptyCreatorProvidedException(string message) : base(message)
        {
        }
    }
}