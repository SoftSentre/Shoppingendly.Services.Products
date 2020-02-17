namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    public class InvalidCreatorEmailException : ShoppingendlyException
    {
        public InvalidCreatorEmailException(string message, params object[] args) : base(message, args)
        {
        }
    }
}