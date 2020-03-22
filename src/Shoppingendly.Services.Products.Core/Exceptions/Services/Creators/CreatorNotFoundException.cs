namespace Shoppingendly.Services.Products.Core.Exceptions.Services.Creators
{
    internal class CreatorNotFoundException : ShoppingendlyException
    {
        internal CreatorNotFoundException(string message) : base(message)
        {
        }
    }
}