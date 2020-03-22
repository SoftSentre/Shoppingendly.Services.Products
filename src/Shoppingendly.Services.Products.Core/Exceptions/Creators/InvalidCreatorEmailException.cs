namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    internal class InvalidCreatorEmailException : ShoppingendlyException
    {
        internal InvalidCreatorEmailException(string message) : base(message)
        {
        }
    }
}