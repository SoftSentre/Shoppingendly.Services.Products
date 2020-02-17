namespace Shoppingendly.Services.Products.Core.Exceptions.Creators
{
    public class InvalidCreatorRoleException : ShoppingendlyException
    {
        public InvalidCreatorRoleException(string message, params object[] args) : base(message, args)
        {
        }
    }
}