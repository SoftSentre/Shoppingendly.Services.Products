namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class PictureCanNotBeEmptyException : ShoppingendlyException
    {
        public PictureCanNotBeEmptyException(string message) : base(message)
        {
        }
    }
}