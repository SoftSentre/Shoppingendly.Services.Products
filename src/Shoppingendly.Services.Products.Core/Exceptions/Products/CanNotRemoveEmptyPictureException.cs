namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    internal class CanNotRemoveEmptyPictureException : ShoppingendlyException
    {
        internal CanNotRemoveEmptyPictureException(string message) : base(message)
        {
        }
    }
}