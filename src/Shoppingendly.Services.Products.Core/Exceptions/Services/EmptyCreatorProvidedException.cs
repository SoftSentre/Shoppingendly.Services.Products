namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    public class EmptyCreatorProvidedException : ShoppingendlyException
    {
        public EmptyCreatorProvidedException(string message, params object[] args) : base(message, args)
        {
        }
    }
}