namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    public class InvalidCreatorNameException : ShoppingendlyException
    {
        public InvalidCreatorNameException(string message, params object[] args) : base(message, args)
        {
        }
    }
}