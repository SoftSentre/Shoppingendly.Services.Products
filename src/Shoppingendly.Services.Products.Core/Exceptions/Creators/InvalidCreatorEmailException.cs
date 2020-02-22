namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    public class InvalidCreatorEmailException : ShoppingendlyException
    {
        public InvalidCreatorEmailException(string message) : base(message)
        {
        }
    }
}