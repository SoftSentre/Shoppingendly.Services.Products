namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class PictureNameIsTooLongException : ShoppingendlyException
    {
        public PictureNameIsTooLongException(string message) : base(message)
        {
        }
    }
}