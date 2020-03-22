namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    internal class InvalidCreatorNameException : ShoppingendlyException
    {
        internal InvalidCreatorNameException(string message) : base(message)
        {
        }
    }
}