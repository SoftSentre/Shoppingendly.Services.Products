namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class CanNotRemoveEmptyPictureException : ShoppingendlyException
    {
        public CanNotRemoveEmptyPictureException(string message) : base(message)
        {
        }
    }
}