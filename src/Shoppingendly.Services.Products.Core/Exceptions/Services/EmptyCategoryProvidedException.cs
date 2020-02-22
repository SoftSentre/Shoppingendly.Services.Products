namespace Shoppingendly.Services.Products.Core.Exceptions.Services
{
    public class EmptyCategoryProvidedException : ShoppingendlyException
    {
        public EmptyCategoryProvidedException(string message) : base(message)
        {
        }
    }
}